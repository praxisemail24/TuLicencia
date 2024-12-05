using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Services;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PagoController
    {
        private readonly PagoService _pagoService;
        private readonly IPagoRepository _repository;
        private readonly IEmailRepository _emailRepository;

        public PagoController(PagoService pagoService, IPagoRepository repository, IEmailRepository er)
        {
            _pagoService = pagoService;
            _repository = repository;
            _emailRepository = er;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<Pago>>> PagarTarjeta(Pago pago)
        {
            ResponseEntity<Pago> result = new ResponseEntity<Pago>();
            ResultadoPago? transaction = _pagoService.AddPago(pago);
            //ResultadoPago transaction = new ResultadoPago();   //   Pruebas sin pagar
            if (transaction != null)
            {
                pago.pg_codigo = transaction.InvoiceNumber;
                result = await _repository.Add(pago, transaction);
                await _emailRepository.EmailPago(pago);
            }
            else {
                result.success = false;
                result.message = "No se ha podigo realizar la transaccion"; 
                
            }           
            return result;
        }



        [HttpPost("PagoDiferido")]
        public async Task<ActionResult<ResponseEntity<Pago>>> PagarTarjetaDiferido(Pago pago)
        {
            ResponseEntity<Pago> result = new ResponseEntity<Pago>();
            try
            {
                ResultadoPago? transaction = _pagoService.AddPagoDiferido(pago);
                //ResultadoPago transaction = new ResultadoPago();   //   Pruebas sin pagar
                
                if (transaction == null)
                    throw new Exception("No se ha podigo realizar la transaccion, verifique que los datos ingresados sean correctos.");

                pago.pg_codigo = transaction.InvoiceNumber;
                result = await _repository.AddPagoMulta(pago, transaction);
                await _emailRepository.EmailPago(pago);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }            
            return result;
        }

        [HttpPost("SinPAGO")]
        public async Task<ActionResult<ResponseEntity<Pago>>> PagarTarjeta2(Pago pago)
        {
            ResponseEntity<Pago> result = new ResponseEntity<Pago>();
            //ResultadoPago transaction = _pagoService.AddPago(pago);
            ResultadoPago transaction = new ResultadoPago();   //   Pruebas sin pagar
            if (transaction != null)
            {
                //result = await _repository.Add(pago, transaction);
                await _emailRepository.EmailPago(pago);
            }
            else
            {
                result.success = false;
                result.message = "No se ha podigo realizar la transaccion";
            }
            return result;
        }


        [HttpPost("Validar")]
        public bool ValidarCuenta(LoginCard loginCard)
        {
            try
            {
                return _pagoService.ValidarCuenta(loginCard);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("{idCliente}")]
        public async Task<ActionResult<ResponseEntity<Pago>>> GetByIdCliente(int idCliente)
        {
            ResponseEntity<Pago> response = new ResponseEntity<Pago>();
            response = await _repository.GetPagoByIdCliente(idCliente);
            return response;
        }

        [HttpGet("CodigoPago/{pg_codigo}")]
        public async Task<ActionResult<ResponseEntity<Pago>>> GetByCodigoPago(string pg_codigo)
        {
            ResponseEntity<Pago> response = new ResponseEntity<Pago>();
            response = await _repository.GetPagoByCodigoPago(pg_codigo);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<Pago>>> GetPago()
        {
            ResponseEntity<Pago> response = new ResponseEntity<Pago>();
            response = await _repository.GetAllPago();
            return response;
        }


        [HttpPost("generarPdf/{pg_codigo}")]
        public async Task<ActionResult<ResponseEntity>> GenerarPDF(string pg_codigo, string nombreArchivo)
        {
            ResponseEntity response = new ResponseEntity();
            try
            {
                await _repository.GenerarReciboDePagoPDF(pg_codigo, nombreArchivo);
                response.message = "PDF generado exitosamente para el pago " + nombreArchivo;
            }
            catch (Exception ex)
            {
                response.message = "Error al generar el PDF: "+ ex.Message;
            }
            return response;
        }


        [HttpGet("CodigoPagoEstado/{pg_codigo}")]
        public async Task<ActionResult<ResponseEntity<Pago>>> GetEstadoByCodigoPago(string pg_codigo)
        {
            ResponseEntity<Pago> response = new ResponseEntity<Pago>();
            response = await _repository.GetEstadoByPago(pg_codigo);
            return response;
        }


    }
}
