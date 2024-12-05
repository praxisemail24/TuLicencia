using SmartLicencia.Entity;
using Stripe;
using Stripe.Checkout;

namespace SmartLicencia.Services
{
    public class CoreStripeService
    {
        //public const string PRIVATE_KEY = "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";
        //public const string PUBLIC_KEY = "pk_live_51QIwjOHSO8iWg0I2a2PDc1A1rlzL2q4YpsVs07gJPzBsYJfXxW1RxD5J3OmS4kfi9VbxKtnpT5L8ppJtiaFDTKlZ004aHwjjxH";
        private readonly string _publicKey;
        private readonly string _privateKey;

        public CoreStripeService(IConfiguration config)
        {
            _privateKey = config.GetValue<string>("Stripe:privateKey");
            _publicKey = config.GetValue<string>("Stripe:publicKey");

            StripeConfiguration.ApiKey = _privateKey;
        }

        public string PublicKey
        {
            get { return _publicKey; }
        }

        public string PrivateKey
        {
            get { return _privateKey; }
        }

        public string? FindCustomer(Customer customer)
        {
            var options = new CustomerSearchOptions
            {
                Query = $"name:'{customer.Name}' AND metadata['email']:'{customer.Email}'",
            };
            var service = new CustomerService();
            var result = service.Search(options);

            if(result.Data.Count > 0)
                return result.Data[0].Id;
            
            return null;
        }

        public string CreateCustomer(Customer customer)
        {
            var options = new CustomerCreateOptions
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                
            };
            var service = new CustomerService();
            var result = service.Create(options);

            return result.Id;
        }

        public string CreateCard(string customerId, string tokenCardId)
        {
            var options = new CustomerPaymentSourceCreateOptions { 
                Source = tokenCardId,
            };
            var service = new CustomerPaymentSourceService();
            var result = service.Create(customerId, options);

            return result.Id;
        }

        public ResultProduct CreateProduct(Product product)
        {
            var options = new ProductCreateOptions { 
                Name = product.Name,
                Description = product.Description,
                Active = true,
                DefaultPriceData = new ProductDefaultPriceDataOptions
                {
                    Currency = product.Currency ?? "USD",
                    UnitAmountDecimal = product.Price * 100,
                    //Recurring = new ProductDefaultPriceDataRecurringOptions
                    //{
                    //    Interval = "day",
                    //    IntervalCount = 1,
                    //},
                    TaxBehavior = "inclusive",
                },
                Metadata = new Dictionary<string, string>
                {
                    { "internal_code", product.InternalCode }
                },
            };
            var service = new ProductService();
            var rs = service.Create(options);

            return new ResultProduct
            {
                PriceId = rs.DefaultPriceId,
                ProductId = rs.Id,
            };
        }

        public ResultProduct? FindProduct(Product product)
        {
            var options = new ProductSearchOptions
            {
                Query = $"active:'true' AND metadata['internal_code']:'{product.InternalCode}'",
            };
            var service = new ProductService();
            var rs = service.Search(options);

            if (rs.Data.Count == 0)
                return null;

            return new ResultProduct
            {
                PriceId = rs.Data[0].DefaultPriceId,
                ProductId = rs.Data[0].Id,
            };
        }

        public Session CreatePaymentIntentEmbed(Product product, string payloadUrl, Dictionary<string, string>? metadata = null)
        {
            ResultProduct? p = FindProduct(product);

            if (p == null)
                p = CreateProduct(product);

            if (metadata == null)
                metadata = new Dictionary<string, string>();

            metadata.Add("product_id", p.ProductId);
            metadata.Add("price_id", p.PriceId);

            var uriBuilder = new UriBuilder(payloadUrl);
            var queryStr = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            queryStr.Add("price_id", p.PriceId);

            uriBuilder.Query = queryStr.ToString();

            payloadUrl = $"{uriBuilder.ToString()}&session_id={{CHECKOUT_SESSION_ID}}";

            var options = new SessionCreateOptions
            {
                UiMode = "embedded",
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = p.PriceId,
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                ReturnUrl = payloadUrl,
                Metadata = metadata,
                PaymentMethodTypes = new List<string> { "card" },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = metadata,
                    CaptureMethod = "automatic",
                    Description = product.Name,
                }
            };
            var service = new SessionService();
            return service.Create(options);
        }

        public async Task<PaymentIntent> CreatePaymentIntent(PaymentRequest request, Dictionary<string, string> ? metadata = null)
        {
            if(metadata == null)
                metadata = new Dictionary<string, string>();

            var paymentMethodService = new PaymentMethodService();
            var paymentMethodOptions = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Token = request.Token,
                },
                BillingDetails = new PaymentMethodBillingDetailsOptions
                {
                    Name = request.HolderName,
                    Email = request.HolderEmail,
                },
            };
            var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntentOptions = new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt64(request.Amount * 100),
                Currency = request.Currency ?? "usd",
                PaymentMethod = paymentMethod.Id,
                Confirm = true,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                },
                Metadata = metadata,
                CaptureMethod = "automatic",
                Description = request.Name,
            };
            return await paymentIntentService.CreateAsync(paymentIntentOptions);
        }

        public Session GetSession(string sessionId)
        {
            var sessionService = new SessionService();
            return sessionService.Get(sessionId);
        }

        public static string RandomCode(int len = 8)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomCode = new char[len];

            for (int i = 0; i < len; i++)
                randomCode[i] = chars[random.Next(chars.Length)];

            return string.Format("{0:HHmm}{1}", DateTime.Now, new string(randomCode));
        }

        public class Address
        {
            public string? City { get; set; }
            public string Country { get; set; }
            public string? Line1 { get; set; }
            public string? Line2 { get; set; }
            public string? PostalCode { get; set; }
            public string? State { get; set; }

            public Address()
            {
                Country = "PR";
            }
        }


        public class Customer
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public Address Address { get; set; }

            public Customer()
            {
                Address = new Address();
            }
        }

        public class Card
        {
            public string? Number { get; set; }
            public string? SecurityCode { get; set; }
            public string Currency { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public Dictionary<string, string> Meta { get; set; }
            public Address Address { get; set; }

            public Card()
            {
                Currency = "USD";
                Meta = new Dictionary<string, string>();
                Address = new Address();
            }
        }

        public class Product
        {
            public string InternalCode { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string? Currency { get; set; }

            public Product()
            {
                InternalCode = string.Empty;
                Name = string.Empty;
                Description = string.Empty;
            }
        }

        public class ResultProduct
        {
            public string ProductId { get; set; }
            public string PriceId { get; set; }

            public ResultProduct()
            {
                ProductId = string.Empty;
                PriceId = string.Empty;
            }
        }
    }
}
