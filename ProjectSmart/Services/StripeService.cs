using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicense.Utils;

namespace SmartLicencia.Services
{
    public class StripeService : CoreStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailrepository;

        public StripeService(IConfiguration configuration, IEmailRepository emailRepository)
            :base(configuration)
        {
            _configuration = configuration;
            _emailrepository = emailRepository;
        }

        public bool ValidarCuenta(LoginCard loginCard)
        {
            return true;
        }

        static bool ValidarCredenciales(string loginId, string transactionKey)
        {
            return true;
        }

        public ResultadoPago? AddPago(Pago pago)
        {
            return ProcesarPago(pago.Monto, pago.NumeroTarjeta, pago.FechaVencimiento, pago.Cvv, pago.Nombre, pago.Apellido, pago.Description, pago.ZipCode, pago.Email, pago.Phone, pago.Direction, pago.City, pago.State);
        }

        public ResultadoPago? AddPagoDiferido(Pago pago)
        {
            return ProcesarPago(pago.Monto, pago.NumeroTarjeta, pago.FechaVencimiento, pago.Cvv, pago.Nombre, pago.Apellido, pago.Description, pago.ZipCode, pago.Email, pago.Phone, pago.Direction, pago.City, pago.State, pago.DetalleCuota, pago.origen, pago.tipo);
        }

        private ResultadoPago? ProcesarPago(decimal monto, string numeroTarjeta, string fechaExpiracion, string codigoSeguridad, string nombre, string apellido,
                                            string description, string zipCode, string email, string phone, string direction, string city, string state, List<Cuota>? cuotas = null, string? origen = null, string? tipo = null)
        {
            ResultadoPago? resultado = null;
            try
            {
                var address = new Address
                {
                    City = city,
                    PostalCode = zipCode,
                    State = state,
                    Line1 = direction,
                };

                var customer = new Customer
                {
                    Name = $"{nombre} {apellido}",
                    Email = email,
                    Phone = phone,
                    Address = address,
                };

                var card = new Card
                {
                    Number = numeroTarjeta,
                    SecurityCode = codigoSeguridad,
                    Address = address,
                };

                string? customerId = FindCustomer(customer);
                
                if(string.IsNullOrWhiteSpace(customerId))
                    customerId = CreateCustomer(customer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return resultado;
        }
    }
}
