using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Services;
using SmartLicencia.Utility;
using System.Text;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentGatewayController : Controller
    {
        private readonly CoreStripeService _stripeService;
        private readonly string _dirRoot;
        private readonly string _baseUrl;
        private readonly string _scriptPath;
        private readonly List<CheckoutRequest> _checkoutItems;

        private const string JWT_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBZG1pbiIsImp0aSI6IjliMDk2NWJlLTQ5MjYtNDU2ZC05OWVkLWZmOThkNDZiMjMzZSIsImV4cCI6MTc0MzgyOTIwMCwiaXNzIjoiUHJheHlUb2tlbnMiLCJhdWQiOiJQcmF4eSJ9.-VNRePV0PvA5agVzg4JSbJkr25aPV2CkVFV1pXqYoDE";

        public PaymentGatewayController(CoreStripeService stripeService, IWebHostEnvironment webHost, ITramiteRepository tramiteRepository)
        {
            _stripeService = stripeService;
            _baseUrl = $"{(webHost.IsDevelopment() ? "https://localhost:7080" : "https://api.tulicenciapr.com")}/api/paymentgateway";
            _dirRoot = webHost.WebRootPath;
            _scriptPath = Path.Combine(_dirRoot, "assets/js/StripePay.js");

            _checkoutItems = new List<CheckoutRequest>();
            var items = tramiteRepository.GetAllTramite().Result.items;

            foreach (var tramite in items)
            {
                _checkoutItems.Add(new CheckoutRequest
                {
                    Code = $"TRAM-{tramite.tr_id.ToString().PadLeft(5, '0')}",
                    Name = tramite.tr_nombre,
                    Amount = tramite.tr_precio,
                    Description = "Compra de trámite.",
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("script")]
        public IActionResult Script()
        {
            string scriptContent = System.IO.File.ReadAllText(_scriptPath);
            scriptContent = scriptContent.Replace("#PUBLIC_KEY#", _stripeService.PublicKey);
            scriptContent = scriptContent.Replace("#BASE_URL#", _baseUrl);
            return Content(scriptContent, "application/javascript");
        }

        [HttpPost("create-session")]
        public IActionResult CreateSession([FromBody] CheckoutRequest request)
        {
            try
            {
                if (request == null)
                    throw new Exception("Se requiere información de pago.");

                var transId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

                var n = 0;
                var rN = int.TryParse(request.Installments, out n);

                if(!rN) {
                    var bytes = Convert.FromBase64String(request.Installments);
                    var decoded = Encoding.UTF8.GetString(bytes);

                    if (string.IsNullOrWhiteSpace(decoded))
                        throw new Exception("Se requiere información de cuotas. Formato 'nro_cuota|fecha|monto,nro_cuota|fecha|monto'");

                    n = 1;
                    request.Installments = decoded;
                }

                var metadata = new Dictionary<string, string>
                {
                    { "installments", n.ToString() },
                    { "installments_list", request.Installments },
                    { "payment_amount", request.Amount.ToString() },
                    { "payment_currency", request.Currency ?? "USD" },
                    { "cl_id", request.ClId.ToString() },
                    { "tr_id", request.TrId.ToString() },
                    { "product_name", request.Name },
                    { "transaction_id", transId },
                };

                if (!string.IsNullOrWhiteSpace(request.PenaltyType))
                    metadata.Add("penalty_type", request.PenaltyType);

                if (!string.IsNullOrWhiteSpace(request.PenaltyOrigin))
                    metadata.Add("penalty_origin", request.PenaltyOrigin);

                if (!string.IsNullOrWhiteSpace(request.FrmId))
                    metadata.Add("frm_id", request.FrmId);

                if (string.IsNullOrWhiteSpace(request.Code))
                    request.Code = Guid.NewGuid().ToString();

                var session = _stripeService.CreatePaymentIntentEmbed(new CoreStripeService.Product
                {
                    InternalCode = request.Code,
                    Name = request.Name,
                    Currency = request.Currency,
                    Description = request.Description ?? request.Name,
                    Price = request.Amount,
                }, request.PayloadUrl, metadata);

                return Json(new { success = true, clientSecret = session.ClientSecret, sessionId = session.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al intentar montar formulario de pago. Detalle: {ex.Message}" });
            }
        }

        private void GenerarPdfPago(string customerName, string customerEmail, Pago pago, IPagoRepository pagoRepository)
        {
            var config = HttpContext.RequestServices.GetService<IConfiguration>();

            if (pagoRepository == null)
                throw new Exception("Error al cargar servicio de registro de pagos.");

            if (pago != null && config != null)
            {
                string fileName = Path.Combine(_dirRoot, $"Pagos/{pago.pg_codigo}.pdf");

                var dirPath = Path.GetDirectoryName(fileName);

                if (!Directory.Exists(dirPath) && !string.IsNullOrWhiteSpace(dirPath))
                    Directory.CreateDirectory(dirPath);

                var pdf = pagoRepository.GenerarReciboDePagoPDFM(fileName, pago);
                var senderMail = new SenderMail(new PlantillaMensajeRepository(config), config);

                senderMail.Send("recibo_pago", new MailPartialBody
                {
                    Files = new Dictionary<string, string>
                    {
                        { fileName, "application/pdf" }
                    },
                    To = new List<SenderMailAddress>
                    {
                        new SenderMailAddress { Name = customerName, Address = customerEmail }
                    },
                    Variables = new Dictionary<string, object>
                    {
                        { "pg_id", pago.pg_id },
                        { "pg_codigo", pago.pg_codigo },
                        { "pg_monto", pago.Monto },
                        { "pg_descripcion", pago.Description },
                        { "cliente_nombre", customerName },
                        { "cliente_correo", customerEmail },
                    }
                });
            }
        }

        [HttpGet("session-status/{session_id}")]
        public async Task<IActionResult> SessionStatus(string session_id)
        {
            var session = _stripeService.GetSession(session_id);
            var response = new CheckoutResponse {
                Session = session,
                CustomerEmail = session.CustomerDetails.Email,
                CustomerName = session.CustomerDetails.Name,
                Status = session.Status,
                PaymentId = session.PaymentIntentId,
                PaymentStatus = session.PaymentStatus,
            };

            if(session.PaymentStatus == "paid" && session.Status == "complete")
            {
                try
                {
                    var clId = Convert.ToInt32(session.Metadata["cl_id"]);
                    var trId = Convert.ToInt32(session.Metadata["tr_id"]);
                    var transId = session.Metadata["transaction_id"];

                    var pagoRepository = HttpContext.RequestServices.GetService<IPagoRepository>();

                    if (pagoRepository == null)
                        throw new Exception($"Error al cargar servicio de registro de pagos.");

                    Pago pago = await pagoRepository.GetPagoPorTransId(transId);

                    if(pago == null)
                    {
                        ResponseEntity<Pago> resultPago;
                        if (Array.Exists(session.Metadata.Keys.ToArray(), x => x == "penalty_origin"))
                        {
                            List<Cuota> cuotas = new List<Cuota>();

                            if(Array.Exists(session.Metadata.Keys.ToArray(), x => x == "installments_list"))
                            {
                                var installments = session.Metadata["installments_list"].Split(",");
                                for (var i = 0; i < installments.Length; i++)
                                {
                                    if (!string.IsNullOrWhiteSpace(installments[i]))
                                    {
                                        var detail = installments[i].Split("|");
                                        if(detail.Length == 3)
                                        {
                                            var cuota = new Cuota();
                                            cuota.nro = detail[0];
                                            cuota.fecha = detail[1];
                                            cuota.monto = detail[2];
                                            cuotas.Add(cuota);
                                        }
                                    }
                                }
                            }

                            pago = new Pago
                            {
                                cl_cliente = new Cliente { cl_id = clId, },
                                tr_tramite = new Tramite { tr_id = trId, },
                                frm_id = session.Metadata["frm_id"],
                                cuotas = session.Metadata["installments"],
                                tipo = session.Metadata["penalty_type"],
                                origen = session.Metadata["penalty_origin"],
                                Monto = Convert.ToDecimal(session.Metadata["payment_amount"]),
                                DetalleCuota = cuotas,
                            };

                            resultPago = await pagoRepository.AddPagoMulta(pago, new ResultadoPago
                            {
                                InvoiceNumber = CoreStripeService.RandomCode(),
                                ResptaTransId = transId,
                            });
                        }
                        else
                        {
                            pago = new Pago
                            {
                                cl_cliente = new Cliente { cl_id = clId, },
                                tr_tramite = new Tramite { tr_id = trId, },
                            };

                            resultPago = await pagoRepository.Add(pago, new ResultadoPago
                            {
                                InvoiceNumber = CoreStripeService.RandomCode(),
                                ResptaTransId = transId,
                            });
                        }                        

                        if (!resultPago.success)
                            throw new Exception(resultPago.message);

                        pago = await pagoRepository.GetPagoPorTransId(transId);

                        GenerarPdfPago(session.CustomerDetails.Name, session.CustomerDetails.Email, pago, pagoRepository);
                    }

                    response.Data = pago;
                    response.WithErrors = false;
                }
                catch (Exception ex)
                {
                    response.WithErrors = true;
                    response.Error = ex.Message;
                }
            }

            return Json(response);
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                var transId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                var metadata = new Dictionary<string, string>
                {
                    { "transaction_id", transId },
                    { "tr_id", request.TrId.ToString() },
                    { "cl_id", request.ClId.ToString() },
                    { "payment_amount", request.Amount.ToString() },
                    { "payment_currency", request.Currency ?? "USD" },
                    { "product_name", request.Name },
                };

                if (!string.IsNullOrWhiteSpace(request.PenaltyType))
                    metadata.Add("penalty_type", request.PenaltyType);

                if (!string.IsNullOrWhiteSpace(request.PenaltyOrigin))
                    metadata.Add("penalty_origin", request.PenaltyOrigin);

                if (!string.IsNullOrWhiteSpace(request.FrmId))
                    metadata.Add("frm_id", request.FrmId);

                var pi = await _stripeService.CreatePaymentIntent(request, metadata);

                var pagoRepository = HttpContext.RequestServices.GetService<IPagoRepository>();

                if (pagoRepository == null)
                    throw new Exception($"Error al cargar servicio de registro de pagos.");

                if(pi.Status == "succeeded" || pi.Status == "processing")
                {
                    Pago pago;
                    ResponseEntity<Pago> resultPago;

                    if (!string.IsNullOrWhiteSpace(request.PenaltyOrigin) && !string.IsNullOrWhiteSpace(request.FrmId) && !string.IsNullOrWhiteSpace(request.Installments))
                    {
                        List<Cuota> cuotas = new List<Cuota>();

                        var installments = request.Installments.Split(",");
                        for (var i = 0; i < installments.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(installments[i]))
                            {
                                var detail = installments[i].Split("|");
                                if (detail.Length == 3)
                                {
                                    var cuota = new Cuota();
                                    cuota.nro = detail[0];
                                    cuota.fecha = detail[1];
                                    cuota.monto = detail[2];
                                    cuotas.Add(cuota);
                                }
                            }
                        }

                        pago = new Pago
                        {
                            cl_cliente = new Cliente { cl_id = request.ClId, },
                            tr_tramite = new Tramite { tr_id = request.TrId, },
                            frm_id = request.FrmId,
                            cuotas = cuotas.Count.ToString(),
                            tipo = request.PenaltyType ?? string.Empty,
                            origen = request.PenaltyOrigin ?? string.Empty,
                            Monto = request.Amount,
                            DetalleCuota = cuotas,
                        };

                        resultPago = await pagoRepository.AddPagoMulta(pago, new ResultadoPago
                        {
                            InvoiceNumber = CoreStripeService.RandomCode(),
                            ResptaTransId = transId,
                        });
                    } else
                    {
                        pago = new Pago
                        {
                            cl_cliente = new Cliente { cl_id = request.ClId, },
                            tr_tramite = new Tramite { tr_id = request.TrId, },
                        };

                        resultPago = await pagoRepository.Add(pago, new ResultadoPago
                        {
                            InvoiceNumber = CoreStripeService.RandomCode(),
                            ResptaTransId = transId,
                        });
                    }

                    pago = await pagoRepository.GetPagoPorTransId(transId);

                    GenerarPdfPago(request.HolderName, request.HolderEmail, pago, pagoRepository);

                    return Json(new { success = true, clientSecret = pi.ClientSecret, paymentId = pi.Id, paymentStatus = pi.Status, pgId = resultPago.extra });
                }

                return Json(new { success = true, clientSecret = pi.ClientSecret, paymentId = pi.Id, paymentStatus = pi.Status, });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("checkout")]
        public IActionResult CheckOut([FromQuery] CheckoutRequest model)
        {
            ViewBag.Token = JWT_TOKEN;
            return View("~/Views/PaymentGateway/CheckOut.cshtml", model);
        }

        [AllowAnonymous]
        [HttpGet("checkout/success")]
        public IActionResult CheckOutSuccess(string session_id)
        {
            ViewBag.Token = JWT_TOKEN;
            return View("~/Views/PaymentGateway/CheckOutSuccess.cshtml");
        }

        [AllowAnonymous]
        [HttpGet("checkout/payments/{tr_id}")]
        public IActionResult PorcedurePayment(int tr_id, int clId, string payloadUrl)
        {
            var tramite = _checkoutItems.Where(x => x.Code == $"TRAM-{tr_id.ToString().PadLeft(5, '0')}").FirstOrDefault();

            if (tramite == null)
                throw new Exception("Trámite no encontrado.");

            tramite.ClId = clId;
            tramite.TrId = tr_id;
            tramite.PayloadUrl = payloadUrl;

            return CheckOut(tramite);
        }
    }
}
