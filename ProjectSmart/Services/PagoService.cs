using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using MailKit.Net.Imap;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace SmartLicencia.Services
{
    public class PagoService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailrepository;

        public PagoService(IConfiguration configuration, IEmailRepository emailRepository) 
        {
            _configuration = configuration;
            _emailrepository = emailRepository;
        }

        public bool ValidarCuenta(LoginCard loginCard)
        {
            bool credencialesValidas = ValidarCredenciales(loginCard.Id, loginCard.Clave);
            Console.WriteLine($"Las credenciales son válidas: {credencialesValidas}");
            return credencialesValidas;
        }

        static bool ValidarCredenciales(string loginId, string transactionKey)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

            // Configura tus credenciales
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = loginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = transactionKey
            };

            // Realiza una solicitud de prueba
            var request = new getMerchantDetailsRequest();
            var controller = new getMerchantDetailsController(request);
            var response = controller.ExecuteWithApiResponse();

            // Verifica la respuesta para determinar la validez de las credenciales
            return response != null && response.messages.resultCode == messageTypeEnum.Ok;
        }

        public ResultadoPago AddPago(Pago pago)
        {
            ResultadoPago resultado = ProcesarPago(pago.Monto, pago.NumeroTarjeta, pago.FechaVencimiento, pago.Cvv, pago.Nombre, pago.Apellido,
                                        pago.Description, pago.ZipCode, pago.Email, pago.Phone, pago.Direction, pago.City, pago.State );
            return resultado;
        }

        public ResultadoPago AddPagoDiferido(Pago pago)
        {
            ResultadoPago resultado = ProcesarPagoDiferido(pago.Monto, pago.NumeroTarjeta, pago.FechaVencimiento, pago.Cvv, pago.Nombre, pago.Apellido,
                                        pago.Description, pago.ZipCode, pago.Email, pago.Phone, pago.Direction, pago.City, pago.State,pago.DetalleCuota,pago.origen,pago.tipo);
            return resultado;
        }

        private static string GenerarInvoice(int length)
        {
            string horaActual = DateTime.Now.ToString("HHmm");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            char[] randomCode = new char[length];             // Crear un código aleatorio de la longitud especificada (length - 4 para los dígitos de la hora)

            for (int i = 0; i < 4; i++)             // Agregar los dígitos de la hora al principio
            {
                randomCode[i] = horaActual[i];
            }

            for (int i = 4; i < length; i++)             // Agregar el resto de los caracteres aleatorios
            {
                randomCode[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomCode);
        }


        private ResultadoPago ProcesarPago(decimal monto, string numeroTarjeta, string fechaExpiracion, string codigoSeguridad, string nombre, string apellido,
                                            string description, string zipCode, string email, string phone, string direction, string city, string state)
        {
            var resultadoPago = new ResultadoPago();
            try
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

                // Configura credenciales
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = "8d9Fx2BgF6",
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = "7r9jU79eQ4s42jLX"
                };

                // Configura transacción
                var creditCard = new creditCardType
                {
                    cardNumber = numeroTarjeta,
                    expirationDate = fechaExpiracion,
                    cardCode = codigoSeguridad
                };

                var billTo = new customerAddressType
                {
                    firstName = nombre,
                    lastName = apellido,
                    address = direction,
                    city = city,
                    state = state,
                    zip = zipCode,
                    country = "Puerto Rico",
                    phoneNumber = phone,
                    email = email,
                };

                var customer = new customerDataType
                {
                    
                    email = email
                };
                var paymentType = new paymentType { Item = creditCard };
                var invoiceNumber = GenerarInvoice(12);
                var transactionRequest = new transactionRequestType     // Crear la transacción de autorización
                {
                    transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                    payment = paymentType,
                    amount = monto,
                    currencyCode = "USD",
                    billTo = billTo,
                    customer= customer,
                    order = new orderType
                    {
                        invoiceNumber = invoiceNumber,
                        description = "Compra de trámite",
                    }
                };
                var createRequest = new createTransactionRequest { transactionRequest = transactionRequest };
                var controller = new createTransactionController(createRequest);
                var response = controller.ExecuteWithApiResponse();

                if (response != null) {
                    var tresponse = response.transactionResponse;
                    if (response.messages.resultCode == messageTypeEnum.Ok && tresponse.responseCode == "1")
                    {
                        var resptaTransId = tresponse.transId;
                        resultadoPago.InvoiceNumber = invoiceNumber;
                        resultadoPago.ResptaTransId = resptaTransId;
                        return resultadoPago;
                    }
                    else if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        String erroes_html = "";
                        foreach (var error in response.transactionResponse.errors)
                        {
                            string mensajeError = _configuration.GetSection("ErroresPago:" + error.errorCode).Value;
                            erroes_html += mensajeError + "</br>";
                            Console.WriteLine($"Error de transacción: {mensajeError}");
                        }
                        _emailrepository.EmailPagoError(email, nombre, apellido, erroes_html);
                        return null;
                    }
                    else
                    {
                        string error = response.messages.message.FirstOrDefault()?.text;
                        Console.WriteLine($"Error en el pago: {response.messages.message.FirstOrDefault()?.text}");
                        return null;
                    }
                }
                else
                {
                    _emailrepository.EmailPagoError(email, nombre, apellido, "Error de Pago");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }


        private ResultadoPago ProcesarPagoDiferido(decimal monto, string numeroTarjeta, string fechaExpiracion, string codigoSeguridad, string nombre, string apellido,
                                            string description, string zipCode, string email, string phone, string direction, string city, string state, List<Cuota> Cuota,string origen,string tipo)
        {
            var resultadoPago = new ResultadoPago();
            try
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

                // Configura credenciales
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = "8d9Fx2BgF6",
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = "7r9jU79eQ4s42jLX"
                };

                // Configura transacción
                var creditCard = new creditCardType
                {
                    cardNumber = numeroTarjeta,
                    expirationDate = fechaExpiracion,
                    cardCode = codigoSeguridad
                };

                var billTo = new customerAddressType
                {
                    firstName = nombre,
                    lastName = apellido,
                    address = "",
                    city = "",
                    state = "",
                    zip = "",
                    country = "Puerto Rico",
                    phoneNumber = "",
                    email = email,
                };
                var customer = new customerDataType
                {

                    email = email
                };
                var paymentType = new paymentType { Item = creditCard };
                var invoiceNumber = "MU" + GenerarInvoice(10);
                var transactionRequest = new transactionRequestType     // Crear la transacción de autorización
                {
                    transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                    payment = paymentType,
                    amount = monto,
                    currencyCode = "USD",
                    billTo = billTo,
                    customer = customer,
                    order = new orderType
                    {
                        invoiceNumber = invoiceNumber,
                        description = "Pago de multa",
                    }
                };
                var createRequest = new createTransactionRequest { transactionRequest = transactionRequest };
                var controller = new createTransactionController(createRequest);
                var response = controller.ExecuteWithApiResponse();

                if (response != null)
                {
                    var tresponse = response.transactionResponse;
                    if (response.messages.resultCode == messageTypeEnum.Ok && tresponse.responseCode == "1")
                    {
                        var resptaTransId = tresponse.transId;
                        resultadoPago.InvoiceNumber = invoiceNumber;
                        resultadoPago.ResptaTransId = resptaTransId;

                        Pago a = new Pago();
                        a.Email = email;
                        a.Nombre = nombre + " " + apellido;
                        a.Monto = monto;
                        a.pg_codigo = resultadoPago.InvoiceNumber;
                        a.Description = "Pago " + origen;
                        Cliente cl = new Cliente();
                        cl.cl_nombre = nombre;
                        cl.cl_primerApellido = apellido;
                        cl.cl_numeroTelefono = "";
                        cl.cl_direccion = "";
                        Tramite tr = new Tramite();
                        tr.tr_nombre = origen;
                        tr.tr_precio = monto;
                        tr.tr_id = 1;
                        tr.tipoparcialtotal= tipo;
                        a.cl_cliente = cl;
                        a.tr_tramite = tr;
                        a.pg_txid = resptaTransId;


                        if (Cuota == null)
                        {
                            
                            _emailrepository.EmailPagoX(a); 
                        }
                        else
                        {
                            _emailrepository.EmailPagoDiferido(email, nombre, apellido, Cuota);
                            _emailrepository.EmailPagoX(a);

                        }

                        return resultadoPago;
                    }
                    else if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        String erroes_html = "";
                        foreach (var error in response.transactionResponse.errors)
                        {
                            string mensajeError = _configuration.GetSection("ErroresPago:" + error.errorCode).Value;
                            erroes_html += mensajeError + "</br>";
                            Console.WriteLine($"Error de transacción: {mensajeError}");
                        }
                        _emailrepository.EmailPagoError(email, nombre, apellido, erroes_html);
                        return null;
                    }
                    else
                    {
                        string error = response.messages.message.FirstOrDefault()?.text;
                        Console.WriteLine($"Error en el pago: {response.messages.message.FirstOrDefault()?.text}");
                        return null;
                    }
                }
                else
                {
                    _emailrepository.EmailPagoError(email, nombre, apellido, "Error de Pago");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }


    }
}
