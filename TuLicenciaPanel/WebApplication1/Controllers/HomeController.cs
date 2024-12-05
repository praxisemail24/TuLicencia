using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebApplication1.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [Authorize(Roles = "Administrador,Operador,Radicador")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("Home/VerFormulario/{cl_id}/{tr_id}/{frm_id}")] //tr 1
        public IActionResult VerFormulario(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View();
        }

        [AllowAnonymous]
        [HttpGet("Home/FormRenovLicReview/{cl_id}/{tr_id}/{frm_id}")] //tr 1
        public IActionResult FormRenovLicReview(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View();
        }

        [HttpGet("Home/VerFormLicReciprocidadNew/{cl_id}/{tr_id}/{frm_id}")] //tr 4
        public IActionResult VerFormLicReciprocidadNew(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View(tr_id);
        }

        [HttpGet("Home/VerFormDupliLicNew/{cl_id}/{tr_id}/{frm_id}")] //tr 3
        public IActionResult VerFormDupliLicNew(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View(tr_id);
        }

        [HttpGet("Home/VerFormDupliLicReview/{cl_id}/{tr_id}/{frm_id}")] //tr 3
        public IActionResult VerFormDupliLicReview(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View(tr_id);
        }

        [HttpGet("Home/VerFormLicReciprocidadReview/{cl_id}/{tr_id}/{frm_id}")] //tr 4
        public IActionResult VerFormLicReciprocidadReview(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View(tr_id);
        }

        [HttpGet("Home/FormCertificadoMed/{cl_id}/{tr_id}")]
        public IActionResult FormCertificadoMed(int cl_id, int tr_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            return View(tr_id);
        }

        [HttpGet("Home/ProcessForm/{cl_id}/{tr_id}/{frm_id}")]
        public IActionResult ProcessForm(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View();
        }

        [HttpGet("Home/ClosedForm/{cl_id}/{tr_id}/{frm_id}")]
        public IActionResult ClosedForm(int cl_id, int tr_id, int frm_id)
        {
            ViewBag.cl_id = cl_id;
            ViewBag.tr_id = tr_id;
            ViewBag.frm_id = frm_id;
            return View();
        }

        [Authorize(Roles = "Doctor")]  // Asegúrate de que el rol es correcto
        [HttpGet("Home/Doctor/{doctorId}")]
        public async Task<IActionResult> Doctor(int doctorId)
        {
            //var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

            //if (doctor == null)
            //{
            //    return NotFound();
            //}

            ViewBag.DoctorId = doctorId;// doctor.Id;
            ViewBag.Nombre = "nombre";//doctor.Nombre;
            ViewBag.Email = "email";//doctor.Email;

            return View();
        }

        [Authorize(Roles = "Doctor")]  // Asegúrate de que el rol es correcto
        [HttpGet("Home/Evaluacion/{id}/{tr_id}")]
        public IActionResult Evaluacion(string id,string tr_id)
        {
            ViewBag.id = id;
            ViewBag.tr_id = tr_id;
            return View("Evaluacion");
        }

        private string FormatDate(object dateObject)
        {
            if (dateObject == null) return " ";

            if (DateTime.TryParse(dateObject.ToString(), out DateTime date))
            {
                return date.ToString("MM/dd/yyyy");
            }
            return " ";
        }

        [AllowAnonymous]
        [HttpGet("Home/VerFormPDFReciprocidad/{cl_id}/{tr_id}")] //tr 1-3
        public IActionResult VerFormPDFReciprocidad(int cl_id, int tr_id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var apiUrl = $"https://api.tulicenciapr.com/api/RenovLic/obtenerDatosForm/{cl_id}/{tr_id}";
                var response = client.GetAsync(apiUrl).Result; // Utilizando Result en lugar de await

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result; // Utilizando Result en lugar de await
                    dynamic data = JsonConvert.DeserializeObject(content);
                    if (data != null)
                    {
                        ViewBag.cl_id = cl_id;
                        ViewBag.tr_id = tr_id;
                        ViewBag.correo = data.item.cl_cliente.cl_correo;
                        ViewBag.nombrePadre = data.item.nombrePadre;
                        ViewBag.nombreMadre = data.item.nombreMadre;
                        ViewBag.paisProcede = data.item.paisProcede;
                        ViewBag.estadoProcede = data.item.estadoProcede;

                        // Categoria
                        ViewBag.numeroLicencia = data.item.numeroLicencia;
                        // Nombres separados por espacio
                        var nombreCompleto = (string)data.item.cl_cliente.cl_nombre;
                        var nombres = nombreCompleto.Split(' ');  // Dividir el nombre completo por el espacio
                        ViewBag.primerNombre = nombres.Length > 0 ? nombres[0] : "";  // Guardar el primer nombre
                        ViewBag.segundoNombre = nombres.Length > 1 ? nombres[1] : "";
                        // Apellidos
                        ViewBag.primerApellido = data.item.cl_cliente.cl_primerApellido;
                        ViewBag.segundoApellido = data.item.cl_cliente.cl_segundoApellido;
                        // Numero identificaacion
                        ViewBag.numeroIdentificacion = data.item.numeroIdentificacion;
                        // Tipo sangre
                        ViewBag.tipoSangre = data.item.tipoSangre;
                        // Numero de telefono quitando el prefino
                        var telefonoUsuario = (string)data.item.cl_cliente.cl_numeroTelefono;
                        var numeroSinCodigo = telefonoUsuario.Replace("(787) ", "");  // Eliminar el código de área
                        ViewBag.numeroTelefonoUsuario = numeroSinCodigo;
                        // Fecha de nacimiento
                        var fechaNacimientoCompleta = (string)data.item.cl_cliente.cl_fechaNacimiento;

                        DateTime fecha = DateTime.Parse(fechaNacimientoCompleta);

                        int mesNacimiento = fecha.Month;
                        int diaNacimiento = fecha.Day;
                        int anioNacimiento = fecha.Year;

                        ViewBag.fechaNacimientoMes = mesNacimiento;
                        ViewBag.fechaNacimientoDia = diaNacimiento;
                        ViewBag.fechaNacimientoAnio = anioNacimiento;
                        // Estatura
                        var estaturaCompleta = (string)data.item.talla; // Asume que esto viene como "4'5\""

                        var partesEstatura = estaturaCompleta.Split('\'');
                        var pies = partesEstatura[0];  // Obtiene el número antes del apóstrofe

                        var pulgadas = partesEstatura.Length > 1 ? partesEstatura[1].Split('\"')[0] : "0"; // Asegura que haya una segunda parte

                        ViewBag.estaturaPies = pies;
                        ViewBag.estaturaPulgadas = pulgadas;
                        // Peso
                        ViewBag.peso = data.item.peso;
                        //Direccion Residencial
                        ViewBag.direccionPrimero = data.item.direccion; // Direccion 1
                        ViewBag.numeroDireccionPrimero = data.item.numeroDireccion; // Numero 1
                        ViewBag.puebloPrimero = data.item.pueblo;  // pueblo 1
                        ViewBag.postalprimero = data.item.codigoPostal; // Cod Postal 1
                        //Direccion Postal
                        ViewBag.direccionSegundo = data.item.barrio; // Direccion 2
                        ViewBag.puebloSegundo = data.item.pueblo2;  // pueblo 2
                        ViewBag.postalSegundo = data.item.codigoPostal2; // Cod Postal 2
                                                                        
                        DateTime fechaActual = DateTime.Now;

                        // Formatear la fecha en el estilo "mes/día/año"
                        ViewBag.FechaDocumento = fechaActual.ToString("MM/dd/yyyy");

                        var tipoLicencia = (string)data.item.tipoLicencia;  // Asegúrate de obtener correctamente los datos

                        ViewBag.OpacidadLicenciaConducir = tipoLicencia == "Licencia de Conducir" ? "1" : "0";
                        ViewBag.OpacidadLicenciaConducirReal = tipoLicencia == "Licencia de Conducir Real ID" ? "1" : "0";

                        // cateogira solicitada + vehiculo pesado
                        ViewBag.isConductor = (string)data.item.categoria == "Conductor";
                        ViewBag.isChofer = (string)data.item.categoria == "Chofer";

                        string tipoVehiculo = (string)data.item.tipoVehiculo; // Esto debe ser string ya

                        ViewBag.isCatVehiculo_1 = ViewBag.isChofer && tipoVehiculo == "1";
                        ViewBag.isCatVehiculo_2 = ViewBag.isChofer && tipoVehiculo == "2";
                        ViewBag.isCatVehiculo_3 = ViewBag.isChofer && tipoVehiculo == "3";
                        ViewBag.isCatVehiculo_4 = ViewBag.isChofer && tipoVehiculo == "4";

                        // SEGURO SOCIAL O PASAPORTE
                        var identificacion = (string)data.item.identificacion;

                        ViewBag.OpacidadPasaporte = identificacion == "Pasaporte" ? "1" : "0";
                        ViewBag.OpacidadSeguroSocial = identificacion == "Seguro Social" ? "1" : "0";

                        // Status legal
                        var statusLegal = (string)data.item.statusLegal;

                        ViewBag.OpacidadResidentePermanente = statusLegal == "Residente Permanente" ? "1" : "0";
                        ViewBag.OpacidadCiudadanoUsa = statusLegal == "Ciudadano USA" ? "1" : "0";
                        ViewBag.OpacidadOtroPresencialLegal = statusLegal == "Otro" ? "1" : "0";

                        // GENERO
                        var genero = (string)data.item.genero;

                        ViewBag.OpacidadGeneroMasculino = genero == "Masculino" ? "1" : "0";
                        ViewBag.OpacidadGeneroFemenino = genero == "Femenino" ? "1" : "0";

                        // Donante organos
                        var donanteOpcion = (string)data.item.donante;

                        ViewBag.OpacidadDonanteSi = donanteOpcion == "Sí" ? "1" : "0";
                        ViewBag.OpacidadDonanteNo = donanteOpcion == "No" ? "1" : "0";

                        // Tez
                        var tez = (string)data.item.tez;
                        ViewBag.OpacidadTezAmarilla = tez == "Amarilla" ? "1" : "0";
                        ViewBag.OpacidadTezBlanca = tez == "Blanca" ? "1" : "0";
                        ViewBag.OpacidadTezNegro = tez == "Negra" ? "1" : "0";

                        // Pelo
                        var pelo = (string)data.item.colorPelo;
                        ViewBag.OpacidadPeloAmarillo = pelo == "Amarillo" ? "1" : "0";
                        ViewBag.OpacidadPeloBlanco = pelo == "Blanco" ? "1" : "0";
                        ViewBag.OpacidadPeloGris = pelo == "Gris" ? "1" : "0";
                        ViewBag.OpacidadPeloMarron = pelo == "Marrón" ? "1" : "0";
                        ViewBag.OpacidadPeloNegro = pelo == "Negro" ? "1" : "0";
                        ViewBag.OpacidadPeloRojo = pelo == "Rojo" ? "1" : "0";
                        ViewBag.OpacidadPeloCalvo = pelo == "Calvo" ? "1" : "0";

                        // Ojos
                        var ojos = (string)data.item.colorOjo;
                        ViewBag.OpacidadOjosAmarillo = ojos == "Amarillo" ? "1" : "0";
                        ViewBag.OpacidadOjosAzul = ojos == "Azul" ? "1" : "0";
                        ViewBag.OpacidadOjosGris = ojos == "Gris" ? "1" : "0";
                        ViewBag.OpacidadOjosMarron = ojos == "Marrón" ? "1" : "0";
                        ViewBag.OpacidadOjosNegro = ojos == "Negro" ? "1" : "0";
                        ViewBag.OpacidadOjosVerde = ojos == "Verde" ? "1" : "0";
                        ViewBag.OpacidadOjosHazel = ojos == "Hazel" ? "1" : "0";

                        // Licencia suspendida
                        var licenciaSuspendida = (string)data.item.licenciaSuspendida;
                        var motivoSuspension = (string)data.item.motivoSuspension;

                        ViewBag.OpacidadSuspendidaSi = licenciaSuspendida == "Sí" ? "1" : "0";
                        ViewBag.OpacidadSuspendidaNo = licenciaSuspendida == "No" ? "1" : "0";

                        var baseOpacity = licenciaSuspendida == "No" ? "0" : "1";
                        ViewBag.OpacidadOpcJudicial = (motivoSuspension == "Judicial" && baseOpacity == "1") ? "1" : "0";
                        ViewBag.OpacidadOpcSistemaPuntos = (motivoSuspension == "Sistema de puntos" && baseOpacity == "1") ? "1" : "0";
                        ViewBag.OpacidadOpcIncapacidad = (motivoSuspension == "Incapacidad" && baseOpacity == "1") ? "1" : "0";
                        ViewBag.OpacidadOpcRenovacionSecretario = (motivoSuspension == "Renovacion del secretario" && baseOpacity == "1") ? "1" : "0";
                        ViewBag.OpacidadOpcLeyAsume = (motivoSuspension == "Ley asume" && baseOpacity == "1") ? "1" : "0";

                        var recluido = (string)data.item.recluido;

                        ViewBag.OpacidadRecluidoMentalSi = recluido == "Sí" ? "1" : "0";
                        ViewBag.OpacidadRecluidoMentalNo = recluido == "No" ? "1" : "0";

                        // FECHAS
                        ViewBag.OpacidadBebidasSi = data.item.convictoBebida == "Sí" ? "1" : "0";
                        ViewBag.OpacidadBebidasNo = data.item.convictoBebida == "No" ? "1" : "0";
                        ViewBag.FechaConvictoBebida = data.item.convictoBebida == "Sí" ? FormatDate2(data.item.fechaConvictoBebida) : " ";

                        ViewBag.OpacidadNarcoticosSi = data.item.convictoNarcotico == "Sí" ? "1" : "0";
                        ViewBag.OpacidadNarcoticosNo = data.item.convictoNarcotico == "No" ? "1" : "0";
                        ViewBag.FechaConvictoNarcotico = data.item.convictoNarcotico == "Sí" ? FormatDate2(data.item.fechaConvictoNarcotico) : " ";

                        ViewBag.OpacidadAsumeSi = data.item.obligacionAlimentaria == "Sí" ? "1" : "0";
                        ViewBag.OpacidadAsumeNo = data.item.obligacionAlimentaria == "No" ? "1" : "0";

                        ViewBag.OpacidadDeudaAccaSi = data.item.deudaAcca == "Sí" ? "1" : "0";
                        ViewBag.OpacidadDeudaAccaNo = data.item.deudaAcca == "No" ? "1" : "0";
                        ViewBag.numeroLicenciaPR = data.item.numeroLicenciaPR;
                        ViewBag.fechaHoy = DateTime.Now.ToString("MM/dd/yyyy");

                        return View("VerPdfReciprocidadLic");
                    }
                    else
                    {
                        _logger.LogError("Error al deserializar la respuesta JSON del API");
                        return RedirectToAction("Error");
                    }
                }
                else
                {
                    _logger.LogError($"Error al enviar la solicitud al API. Código de estado: {response.StatusCode}");
                    return RedirectToAction("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al API");
                return RedirectToAction("Error");
            }
        }


        private string FormatDate2(object dateObject)
        {
            if (dateObject == null) return " "; 

            if (DateTime.TryParse(dateObject.ToString(), out DateTime date))
            {
                return date.ToString("MM/dd/yyyy");
            }
            return " "; 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("Home/EditarTraspaso/{id}")]
        public IActionResult EditarTraspaso(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpGet("Home/VerTraspaso/{id}")]
        public IActionResult VerTraspaso(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
