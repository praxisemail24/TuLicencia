using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicense.Pdf;
using SmartLicense.Pdf.Models;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TraspasoController : BuilderPDFController
    {
        private readonly TraspasoRepository _traspasoRepository;
        private readonly IClienteRepository _clienteRepository;
        public TraspasoController(IConfiguration config, IWebHostEnvironment web, IClienteRepository clienteRepository)
            : base(web, config)
        {
            _traspasoRepository = new TraspasoRepository(config);
            _clienteRepository = clienteRepository;
        }

        private async Task<Traspaso> __store(TraspasoRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("Verifique que los datos ingresados sean correctos.");

            ResponseEntity<Cliente> responseSeller;
            ResponseEntity<Cliente> responseBuyer;
            var seller = new Cliente
            {
                cl_id = request.Seller.cl_id,
                cl_nombre = request.Seller.cl_nombre ?? string.Empty,
                cl_segundoNombre = request.Seller.cl_segundoNombre ?? string.Empty,
                cl_primerApellido = request.Seller.cl_primerApellido ?? string.Empty,
                cl_segundoApellido = request.Seller.cl_segundoApellido ?? string.Empty,
                cl_numeroSeguro = request.Seller.cl_numeroSeguro ?? string.Empty,
                cl_zip = request.Seller.cl_zip ?? string.Empty,
                cl_correo = request.Seller.cl_correo ?? string.Empty,
                cl_direccion = request.Seller.cl_direccion ?? string.Empty,
                cl_numeroTelefono = request.Seller.cl_numeroTelefono ?? string.Empty,
                cl_estado = 1,
                cl_pueblo = request.Seller.cl_pueblo ?? new Pueblos(),
                cl_numeroLicencia = request.Seller.cl_numeroLicencia ?? string.Empty,
            };

            if (request.Seller.cl_id == 0)
            {
                responseSeller = await _clienteRepository.AddCliente(seller);
                seller.cl_id = Convert.ToInt32(responseSeller.extra);
            } else
            {
                responseSeller = await _clienteRepository.UpdateCliente(seller);
            }

            if (!responseSeller.success)
                throw new Exception(responseSeller.message);

            var buyer = new Cliente
            {
                cl_id = request.Buyer.cl_id,
                cl_nombre = request.Buyer.cl_nombre ?? string.Empty,
                cl_segundoNombre = request.Buyer.cl_segundoNombre ?? string.Empty,
                cl_primerApellido = request.Buyer.cl_primerApellido ?? string.Empty,
                cl_segundoApellido = request.Buyer.cl_segundoApellido ?? string.Empty,
                cl_numeroSeguro = request.Buyer.cl_numeroSeguro ?? string.Empty,
                cl_zip = request.Buyer.cl_zip ?? string.Empty,
                cl_correo = request.Buyer.cl_correo ?? string.Empty,
                cl_direccion = request.Buyer.cl_direccion ?? string.Empty,
                cl_numeroTelefono = request.Buyer.cl_numeroTelefono ?? string.Empty,
                cl_estado = 1,
                cl_pueblo = request.Buyer.cl_pueblo ?? new Pueblos(),
                cl_numeroLicencia = request.Seller.cl_numeroLicencia ?? string.Empty,
            };

            if (request.Buyer.cl_id == 0)
            {
                responseBuyer = await _clienteRepository.AddCliente(buyer);
                buyer.cl_id = Convert.ToInt32(responseBuyer.extra);
            }
            else
            {
                responseBuyer = await _clienteRepository.UpdateCliente(buyer);
            }

            if (!responseBuyer.success)
                throw new Exception(responseBuyer.message);

            return _traspasoRepository.Store(new Traspaso
            {
                Id = request.Id,
                BrandVehicle = request.BrandVehicle,
                Currency = request.Currency,
                OtherInfo = request.OtherInfo,
                ColorVehicle = request.ColorVehicle,
                HasContract = request.HasContract,
                ContractDate = request.ContractDate,
                PgId = request.PgId,
                SerialNumber = request.SerialNumber,
                State = request.State,
                TransferType = request.TransferType,
                YearVehicle = request.YearVehicle,
                PaymentAmount = request.PaymentAmount,
                ModelVehicle = request.ModelVehicle,
                PaymentDate = request.PaymentDate,
                LicensePlate = request.LicensePlate,
                TprId = 2,
                TrId = 5,
                Seller = new Cliente
                {
                    cl_id = seller.cl_id,
                },
                Buyer = new Cliente
                {
                    cl_id = buyer.cl_id,
                },
                AuthorId = request.AuthorId,
            });
        }

        [HttpPost("store")]
        public async Task<ResponseJSONModel<Traspaso>> Store([FromBody] TraspasoRequest request)
        {
            ResponseJSONModel<Traspaso> response = new ResponseJSONModel<Traspaso>();
            try
            {
                var r = await __store(request);

                response.Success = true;
                response.Message = "Se ha guardado correctamente el registro.";
                response.Data = r;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPut("update")]
        public async Task<ResponseJSONModel<Traspaso>> Update([FromBody] TraspasoRequest request)
        {
            ResponseJSONModel<Traspaso> response = new ResponseJSONModel<Traspaso>();
            try
            {
                var r = await __store(request);

                response.Success = true;
                response.Message = "Se ha actualizado correctamente el registro.";
                response.Data = r;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpGet("{id}/show")]
        public async Task<ResponseJSONModel<Traspaso>> Show(int id)
        {
            ResponseJSONModel<Traspaso> response = new ResponseJSONModel<Traspaso>();
            try
            {
                var clienteRepository = HttpContext.RequestServices.GetService<IClienteRepository>();

                if (clienteRepository == null)
                    throw new Exception("Error al intentar cargar servicio de clientes.");

                var model = _traspasoRepository.GetById(id);

                var seller = await clienteRepository.GetClienteById(model.Seller.cl_id);
                var buyer = await clienteRepository.GetClienteById(model.Buyer.cl_id);

                if(seller.item != null)
                {
                    model.Seller.cl_fechaNacimiento = seller.item.cl_fechaNacimiento;
                    model.Seller.cl_estado = seller.item.cl_estado;
                    model.Seller.cl_genero = seller.item.cl_genero;
                    model.Seller.cl_numeroSeguro = seller.item.cl_numeroSeguro;
                    model.Seller.cl_numeroLicencia = seller.item.cl_numeroLicencia;
                    model.Seller.cl_numeroTelefono = seller.item.cl_numeroTelefono;
                    model.Seller.cl_codigoPostal = seller.item.cl_codigoPostal;
                    model.Seller.cl_colorPelo = seller.item.cl_colorPelo;
                    model.Seller.cl_colorOjo = seller.item.cl_colorOjo;
                    model.Seller.cl_peso = seller.item.cl_peso;
                }

                if (buyer.item != null)
                {
                    model.Buyer.cl_fechaNacimiento = buyer.item.cl_fechaNacimiento;
                    model.Buyer.cl_estado = buyer.item.cl_estado;
                    model.Buyer.cl_genero = buyer.item.cl_genero;
                    model.Buyer.cl_numeroSeguro = buyer.item.cl_numeroSeguro;
                    model.Buyer.cl_numeroLicencia = buyer.item.cl_numeroLicencia;
                    model.Buyer.cl_numeroTelefono = buyer.item.cl_numeroTelefono;
                    model.Buyer.cl_codigoPostal = buyer.item.cl_codigoPostal;
                    model.Buyer.cl_colorPelo = buyer.item.cl_colorPelo;
                    model.Buyer.cl_colorOjo = buyer.item.cl_colorOjo;
                    model.Buyer.cl_peso = buyer.item.cl_peso;
                }

                model.Documents = _traspasoRepository.GetDocuments(id);

                response.Success = true;
                response.Message = "Información de traspaso.";
                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpGet("{id}/documents")]
        public ResponseJSONModel<List<TraspasoDoc>> Documents(int id)
        {
            ResponseJSONModel<List<TraspasoDoc>> response = new ResponseJSONModel<List<TraspasoDoc>>();
            try
            {
                var model = _traspasoRepository.GetDocuments(id);

                response.Success = true;
                response.Message = "Información de traspaso.";
                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPost("change-case")]
        public ResponseJSON ChangeCase([FromBody] TraspasosStatus request)
        {
            var response = new ResponseJSON();
            try
            {
                if ((request.Id == null || request.Id == 0) && request.State != null)
                    throw new Exception("Se requiere parámetros de traspaso.");

                var r = _traspasoRepository.ChangeCase(request.Id ?? 0, request.State ?? 0);

                if (!r)
                    throw new Exception("Error al intentar cambiar el caso del traspaso.");

                response.Success = true;
                response.Message = "Se ha actualizado correctamente el traspaso.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPut("change-status/{id}")]
        public ResponseJSON ChangeStatus(long id, [FromBody] TraspasosStatus request)
        {
            var response = new ResponseJSON();
            try
            {
                var r = _traspasoRepository.ChangeStatus(id, request.Revised, request.Evaluation, DateTime.Now, DateTime.Now);

                if (!r)
                    throw new Exception("Error al intentar cambiar el estado del traspaso.");

                response.Success = true;
                response.Message = "Se ha actualizado correctamente el estado del traspaso.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPut("radicator-assign/{id}")]
        public ResponseJSON RadicatorAssign(long id, [FromBody] TraspasosStatus request)
        {
            var response = new ResponseJSON();
            try
            {
                var r = _traspasoRepository.RadicatorAsigned(id, request.AdminId, request.RadicatorId, DateTime.Now);

                if (!r)
                    throw new Exception("Error al intentar asignar radicador.");

                var cS = _traspasoRepository.ChangeCase(id, 1);

                if (!cS)
                    throw new Exception("Error al cambiar el estado del caso.");

                response.Success = true;
                response.Message = "Se ha asignado correctamente un radicador.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPost("doc-radicated")]
        public ResponseJSON Radicated(TraspasosStatus request)
        {
            var response = new ResponseJSON();
            try
            {
                if (request.Id == null || request.Id == 0)
                    throw new Exception("Se requiere identificador de traspaso.");

                var r = _traspasoRepository.Radicated(request.Id ?? 0, request.Radicated ?? true, DateTime.Now);

                if (!r)
                    throw new Exception("Error al intentar radicar el traspaso.");

                var cS = _traspasoRepository.ChangeCase(request.Id ?? 0, 2);

                if (!cS)
                    throw new Exception("Error al cambiar el estado del caso.");

                response.Success = true;
                response.Message = "Se ha radicado correctamente el traspaso.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }

        [HttpPost("radication-state")]
        public ResponseJSON RadicatorAssign([FromBody] TraspasosStatus request)
        {
            var response = new ResponseJSON();
            try
            {
                if(request.Id == null || request.Id == 0)
                    throw new Exception("Se requiere identificador de traspaso.");

                var r = _traspasoRepository.RadicationStatus(request.Id ?? 0, request.RadicationState, request.RadicationObservation);

                if (!r)
                    throw new Exception("Error al intentar actualizar el estado de radicación.");

                response.Success = true;
                response.Message = "Se ha asignado correctamente un radicador.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Error = ex;
            }
            return response;
        }


        /// <summary>
        /// Busca vendedor o comprador en la tabla cliente
        /// </summary>
        /// <param name="column">ssn, email, number-license, given-name, sur-name</param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-seller-buyer/{column}")]
        public ResponseJSONModel<List<Cliente>> SearchSellerBuyer(string column, string query)
        {
            var response = new ResponseJSONModel<List<Cliente>>();
            try
            {
                var columns = new Dictionary<string, string>();
                columns.Add("ssn", "cl_numeroSeguro");
                columns.Add("email", "cl_correo");
                columns.Add("number-license", "cl_numeroLicencia");
                columns.Add("given-name", "cl_nombre");
                columns.Add("sur-name", "cl_primerApellido");

                if (!Array.Exists(columns.Keys.ToArray(), x => x == column))
                    throw new Exception("Parámetro incorrecto, solo se aceptan los valores (ssn, email, number_license, given_name, sur_name)");

                var rs = _clienteRepository.Search(columns.GetValueOrDefault(column) ?? "cl_correo", query);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        [HttpGet("billofsale/{id}")]
        [HttpGet("gen-billofsale/{id}/{regenerate}")]
        [AllowAnonymous]
        public async Task<FileStreamResult> GenerateBillOfSale(int id, int regenerate)
        {
            string rootDirectory = Path.Combine(_hostEnv.WebRootPath, "BillOfSale");

            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            var fileName = Path.Combine(rootDirectory, $"BillOfSale_{id}.pdf");

            if (!System.IO.File.Exists(fileName))
                regenerate = 1;

            if (regenerate == 1)
            {
                var model = _traspasoRepository.GetById(id);
                var docs = _traspasoRepository.GetDocuments(id);

                var sellerCity = $"{model.Seller.cl_pueblo.pl_nombre}, {model.Seller.cl_zip}";
                var buyerCity = $"{model.Buyer.cl_pueblo.pl_nombre}, {model.Buyer.cl_zip}";

                fileName = await GeneratePdfOfTemplate("BillOfSale", fileName, new BillOfSalePdf
                {
                    Description = "Vehicle Transfer",
                    SellerName = model.Seller.cl_nombreCompleto,
                    SellerAddress = model.Seller.cl_direccion,
                    SellerCity = sellerCity,
                    SerialNumber = model.SerialNumber ?? string.Empty,
                    Make = model.BrandVehicle ?? string.Empty,
                    Model = model.ModelVehicle ?? string.Empty,
                    OtherInfo = model.OtherInfo ?? string.Empty,
                    BuyerName = model.Buyer.cl_nombreCompleto,
                    BuyerAddress = model.Buyer.cl_direccion,
                    BuyerCity = buyerCity,
                    PaymentAmount = (model.PaymentAmount ?? 0m).ToString("N2"),
                    PaymentDate = model.PaymentDate.HasValue ? string.Format("{0:MM/dd/yyyy}", model.PaymentDate.Value) : string.Empty,
                });
            }

            return renderPdf(fileName);
        }


    }
}
