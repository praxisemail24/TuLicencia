using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using SmartLicencia.Services;


namespace SmartLicencia.Repository
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        private readonly IConfiguration _configuration;

        public PagoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Pago>> Add(Pago pago, ResultadoPago resultado)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", pago.cl_cliente.cl_id },
                    { "tr_id", pago.tr_tramite.tr_id },
                    { "pg_fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "pg_codigo", resultado.InvoiceNumber },
                    //{ "pg_codigo", pago.pg_codigo },
                    { "pg_estado", "1" },
                    { "pg_nota", "" },
                    { "pg_status", "codAutorize" },
                    { "pg_txid", resultado.ResptaTransId },
                    //{ "pg_txid", "45454" },
                    { "pg_metodo", "1" },
                };  
                ResponseEntity<Pago> result = await Add(pago, "sp_RegistrarPago", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Pago>> AddPagoMulta(Pago pago, ResultadoPago resultado)
        {
            if (resultado == null)
            {
                ResultadoPago resultadox = new ResultadoPago();
                resultadox.InvoiceNumber = "xxxxxx";
                resultadox.ResptaTransId = "xxxxxx";
                resultado = resultadox;
            }

            var parameters = new Dictionary<string, object>
            {
                { "cl_id", pago.cl_cliente.cl_id },
                { "tr_id", pago.tr_tramite.tr_id },
                { "pg_fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "pg_codigo", resultado.InvoiceNumber },
                //{ "pg_codigo", pago.pg_codigo },
                { "pg_estado", "1" },
                { "pg_nota", "" },
                { "pg_status", "codAutorize" },
                { "pg_txid", resultado.ResptaTransId },
                //{ "pg_txid", "45454" },
                { "pg_metodo", "1" },
                {"tipo" ,pago.tipo},
                {"origen" ,pago.origen},
                {"cuotas" ,pago.cuotas},
                {"total" ,pago.Monto},
                {"frm_id" ,pago.frm_id},
            };
            ResponseEntity<Pago> result = await Add(pago, "sp_RegistrarPagoMulta", parameters);

            try
            {
                if (pago.DetalleCuota != null)
                {
                    foreach (Cuota x in pago.DetalleCuota)
                    {
                        var parameters1 = new Dictionary<string, object>
                        {
                            { "multapagoid", result.extra},
                            { "monto", x.monto },
                            { "nrocuota", x.nro },

                        };
                        ResponseEntity<Pago> result1 = await Add(pago, "sp_RegistrarMultaCuota", parameters1);

                    }
                }
            }
            catch (Exception)
            {

            }
            

            return result;


        }


        public async Task<ResponseEntity<Pago>> GetPagoByIdCliente(int cl_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_id"] = cl_id;
                ResponseEntity<Pago> result = await GetAllDataById("sp_ObtenerPagoPorIdCliente", (SqlDataReader dr) =>
                {
                    return new Pago()
                    {
                        pg_id = Convert.ToInt32(dr["pg_id"].ToString()),
                        cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                        tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        pg_fecha = Convert.ToDateTime(dr["pg_fecha"].ToString()),
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_estado = Convert.ToInt32(dr["pg_estado"].ToString()),
                        pg_nota = dr["pg_nota"].ToString(),
                        pg_status = dr["pg_status"].ToString(),
                        pg_txid = dr["pg_txid"].ToString(),
                        pg_metodo = Convert.ToInt32(dr["pg_metodo"].ToString()),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Pago>> GetPagoByCodigoPago(string pg_codigo)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@pg_codigo"] = pg_codigo;
                ResponseEntity<Pago> result = await GetAllDataById("ObtenerPagoPorCodigoPago", (SqlDataReader dr) =>
                {
                    return new Pago()
                    {
                        pg_id = Convert.ToInt32(dr["pg_id"].ToString()),
                        cl_cliente = new Cliente
                        {
                            cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                            cl_nombre = dr["cl_nombre"].ToString(),
                            cl_primerApellido = dr["cl_primerApellido"].ToString(),
                            cl_direccion = dr["cl_direccion"].ToString(),
                            //cl_zip = dr["cl_zip"].ToString(),
                            cl_numeroTelefono = dr["cl_numeroTelefono"].ToString(),
                        },
                        tr_tramite = new Tramite
                        {
                            tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]),
                            tr_nombre = dr["tr_nombre"].ToString(),
                            tr_precio = Convert.ToInt32(dr["tr_precio"].ToString()),
                        },
                        pg_fecha = Convert.ToDateTime(dr["pg_fecha"].ToString()),
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_estado = Convert.ToInt32(dr["pg_estado"].ToString()),
                        pg_nota = dr["pg_nota"].ToString(),
                        pg_status = dr["pg_status"].ToString(),
                        pg_txid = dr["pg_txid"].ToString(),
                        pg_metodo = Convert.ToInt32(dr["pg_metodo"].ToString()),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Pago>> GetAllPago()
        {
            try
            {
                ResponseEntity<Pago> result = await GetAllData("sp_ObtenerPago", (SqlDataReader dr) =>
                {
                    return new Pago()
                    {
                        pg_id = Convert.ToInt32(dr["pg_id"].ToString()),
                        cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                        tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        pg_fecha = Convert.ToDateTime(dr["pg_fecha"].ToString()),
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_estado = Convert.ToInt32(dr["pg_estado"].ToString()),
                        pg_nota = dr["pg_nota"].ToString(),
                        pg_status = dr["pg_status"].ToString(),
                        pg_metodo = Convert.ToInt32(dr["pg_metodo"].ToString()),
                        pg_txid = dr["pg_txid"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pago> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public ResponseEntity GenerarReciboDePagoPDFM(string nombreArchivo, Pago pago)
        {
            if (nombreArchivo == null)
            {
                nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
            }

            ResponseEntity response = new ResponseEntity();
            try
            {
                if (pago != null)
                {
                    if (string.IsNullOrWhiteSpace(pago.cl_cliente.cl_nombreCompleto))
                        pago.cl_cliente.cl_nombreCompleto = $"{pago.cl_cliente.cl_nombre} {pago.cl_cliente.cl_primerApellido} {pago.cl_cliente.cl_segundoApellido}";

                    //Cliente c1 = new Cliente();

                    //c1.cl_nombre = "xxx";
                    //c1.cl_primerApellido = "xxx";
                    //c1.cl_numeroTelefono = "xxx";
                    //c1.cl_direccion = "xxx";
                    //pago.cl_cliente = c1;

                    //Tramite t1 = new Tramite();

                    //t1.tr_id = 423434;
                    //t1.tr_nombre = "lciencias dsad";
                    //t1.tr_precio = 24234;
                    //pago.tr_tramite = t1;

                    //Pago pago = pagoResponse.items.FirstOrDefault();
                    string contenidoHTML = $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                    <meta charset=""UTF-8"">
                    <title>ExportPDF</title>
                    <style>
                        body {{
                            font-size: 11px;
                            font-family: sans-serif;
                            line-height: 1.6;
                        }}        
                        .container {{
                            max-width: 800px;
                            margin: 0 auto;
                            padding: 0 20px;                                    
                        }}
                        .info-section ul {{
                            margin-top: 10px;
                            width: 300px; 
                            float: left;
                            list-style: none;
                            padding: 0;
                        }}
                        .second-info-section ul {{
                            list-style: none;
                            margin-top: 28px;
                            margin-left: 420px;
                        }}
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin-top: 20px;
                        }}                                
                        table, th, td {{
                            border: 1px solid #ddd;
                        }}
                        th, td {{
                            padding: 10px;
                            text-align: left;
                        }}
                        .total-section {{
                            margin-top: 20px;
                            margin-right: 70px;
                            text-align: right;
                            font-weight: bold;
                        }}
                    </style>
                </head>

                <body>
                    <div class=""container"">
                        <img src='https://tulicenciapr.com/upload/20240730071018_logotulicencia.png' alt=""Logotipo"">
                        <div class=""info-section"">
                            <ul>
                                <li><strong>Hola, {pago.cl_cliente.cl_nombreCompleto}</strong></li>
                                <li>Muchas gracias por el realizar el pago de tus multas. </li>
                                <li>Nuestra empresa se compromete a proporcionarle un excelente servicio al cliente para cada transacción que usted realice.</li>
                            </ul>
                        </div>

                        <div class=""second-info-section"">
                            <ul>
                                <li> <strong> Fecha: </strong> {pago.pg_fecha.ToShortDateString()}</li>
                                <li> <strong> Estado: </strong> Aprobado </li>
                                <li> <strong> Código: </strong> {pago.pg_txid} </li>
                                <li> <strong> Transacción ID: </strong> {pago.pg_codigo} </li>
                            </ul>
                        </div>

                        <div class=""second-info-section"">
                            <ul>
                                <li> <strong> Dirección: </strong> {pago.cl_cliente.cl_direccion} </li>
                                <li> <strong> T.(Phone): </strong> {pago.cl_cliente.cl_numeroTelefono}</li>
                            </ul>
                        </div>

                        <table>
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Detalle de Multas</th>
                                    <th>Tipo</th>
                                    <th>Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>1</td>
                                    <td>{pago.tr_tramite.tr_nombre}</td>
                                    <td>${pago.tr_tramite.tipoparcialtotal}</td>
                                    <td>${pago.tr_tramite.tr_precio}</td>
                                </tr>
                            </tbody>
                        </table>

                        <div class=""total-section"">
                            <p>Total: ${pago.tr_tramite.tr_precio}</p>
                        </div>

                        <div style=""width: 300px;"">
                            <h4>Nota:</h4>
                            <span>
                                El pago fue realizado desde una tarjeta de crédito. La empresa tiene una política de devolución inmediata en caso usted no haya realizado la transacción. Si tiene alguna duda, no olvide comunicarse con nosotros al siguiente número telefónico: (777) 700-0000.
                            </span>
                        </div>
                    </div>
                </body>
                </html>";

                    //string nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                    //string nombreArchivo = $"pago_{pg_codigo}.pdf";
                    string rutaArchivoPDF = Path.Combine("", nombreArchivo);
                    GenerarPDFDesdeHTML(contenidoHTML, rutaArchivoPDF);
                    Console.WriteLine($"PDF generado exitosamente en: {rutaArchivoPDF}");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el PDF: {ex.Message}");
            }
            return response;
        }
        public async Task<ResponseEntity>GenerarReciboDePagoPDF(string pg_codigo, string nombreArchivo)
        {
            if (nombreArchivo == null ) { 
                nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
            }

            ResponseEntity response = new ResponseEntity();
            try
            {
                var pagoResponse = await GetPagoByCodigoPago(pg_codigo);

                if (pagoResponse.success && pagoResponse.items.Count() > 0)
                {
                    Pago pago = pagoResponse.items.FirstOrDefault();
                    string contenidoHTML = $@"
                        <!DOCTYPE html>
                        <html lang=""es"">
                        <head>
                            <meta charset=""UTF-8"">
                            <title>ExportPDF</title>
                            <style>
                                body {{
                                    font-size: 11px;
                                    font-family: sans-serif;
                                    line-height: 1.6;
                                }}        
                                .container {{
                                    max-width: 800px;
                                    margin: 0 auto;
                                    padding: 0 20px;                                    
                                }}
                                .info-section ul {{
                                    margin-top: 10px;
                                    width: 300px; 
                                    float: left;
                                    list-style: none;
                                    padding: 0;
                                }}
                                .second-info-section ul {{
                                    list-style: none;
                                    margin-top: 28px;
                                    margin-left: 420px;
                                }}
                                table {{
                                    width: 100%;
                                    border-collapse: collapse;
                                    margin-top: 20px;
                                }}                                
                                table, th, td {{
                                    border: 1px solid #ddd;
                                }}
                                th, td {{
                                    padding: 10px;
                                    text-align: left;
                                }}
                                .total-section {{
                                    margin-top: 20px;
                                    margin-right: 70px;
                                    text-align: right;
                                    font-weight: bold;
                                }}
                            </style>
                        </head>

                        <body>
                            <div class=""container"">
                                <img src='https://tulicenciapr.com/upload/20240730071018_logotulicencia.png' alt=""Logotipo"">
                                <div class=""info-section"">
                                    <ul>
                                        <li><strong>Hola, {pago.cl_cliente.cl_nombre + " " + pago.cl_cliente.cl_primerApellido}</strong></li>
                                        <li>Muchas gracias por comprar tu trámite: <strong>{" " + pago.tr_tramite.tr_nombre}</strong></li>
                                        <li>Nuestra empresa se compromete a proporcionarle un excelente servicio al cliente para cada transacción que usted realice.</li>
                                    </ul>
                                </div>

                                <div class=""second-info-section"">
                                    <ul>
                                        <li> <strong> Fecha: </strong> {pago.pg_fecha.ToShortDateString()}</li>
                                        <li> <strong> Estado: </strong> Aprobado </li>
                                        <li> <strong> Código: </strong> {pago.pg_txid} </li>
                                        <li> <strong> Transacción ID: </strong> {pago.pg_codigo} </li>
                                    </ul>
                                </div>

                                <div class=""second-info-section"">
                                    <ul>
                                        <li> <strong> Dirección: </strong> {pago.cl_cliente.cl_direccion} </li>
                                        <li> <strong> T.(Phone): </strong> {pago.cl_cliente.cl_numeroTelefono}</li>
                                    </ul>
                                </div>

                                <table>
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Trámite</th>
                                            <th>Cantidad</th>
                                            <th>Precio</th>
                                            <th>Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1</td>
                                            <td>{pago.tr_tramite.tr_nombre}</td>
                                            <td>1</td>
                                            <td>${pago.tr_tramite.tr_precio}</td>
                                            <td>${pago.tr_tramite.tr_precio}</td>
                                        </tr>
                                    </tbody>
                                </table>

                                <div class=""total-section"">
                                    <p>Total: ${pago.tr_tramite.tr_precio}</p>
                                </div>

                                <div style=""width: 300px;"">
                                    <h4>Nota:</h4>
                                    <span>
                                        El pago fue realizado desde una tarjeta de crédito. La empresa tiene una política de devolución inmediata en caso usted no haya realizado la transacción. Si tiene alguna duda, no olvide comunicarse con nosotros al siguiente número telefónico: (777) 700-0000.
                                    </span>
                                </div>
                            </div>
                        </body>
                        </html>";

                    //string nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                    //string nombreArchivo = $"pago_{pg_codigo}.pdf";
                    string rutaArchivoPDF = Path.Combine("", nombreArchivo);
                    GenerarPDFDesdeHTML(contenidoHTML, rutaArchivoPDF);
                    Console.WriteLine($"PDF generado exitosamente en: {rutaArchivoPDF}");
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el PDF: {ex.Message}");                
            }
            return response;
        }



        public async Task<ResponseEntity> GenerarEvaluacionMedicaPDF(string idEvaluacion,string tr_id, string nombreArchivo, EvaluacionRequest evaluacion)
        {
            if (nombreArchivo == null)
            {
                nombreArchivo = $"evaluacion_medica_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
            }

            ResponseEntity response = new ResponseEntity();
            try
            {
              


                if (evaluacion != null)
                {
                    string contenidoHTML = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""es"" lang=""es"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title>Certificación Médica</title>
    <style type=""text/css""> 
        * {{ margin: 0; padding: 0; text-indent: 0; }}
        body {{ margin-left: 0.5cm; margin-right: 0.5cm; }}
        .s1 {{ color: black; font-family: Tahoma, sans-serif; font-style: normal; font-weight: bold; text-decoration: none; font-size: 14pt; }}
        .s2 {{ color: black; font-family: Tahoma, sans-serif; font-style: normal; font-weight: normal; text-decoration: none; font-size: 10pt; }}
        .s3 {{ color: black; font-family: Tahoma, sans-serif; font-style: normal; font-weight: normal; text-decoration: none; font-size: 9pt; }}
        .s4 {{ color: black; font-family: Tahoma, sans-serif; font-style: normal; font-weight: bold; text-decoration: none; font-size: 9pt; }}
        p {{ color: black; font-family: Tahoma, sans-serif; font-style: normal; font-weight: normal; text-decoration: none; font-size: 9pt; margin: 0pt; text-align: justify; }}
        table, tbody {{ vertical-align: top; overflow: visible; width: 100%; }}
        .underline {{ text-decoration: underline; }}
        .line {{ border-bottom: 1px solid black; width: 100%; display: inline-block; margin-bottom: 5px; }}
        .checkbox {{ display: inline-block; width: 12px; height: 12px; border: 1px solid black; text-align: center; line-height: 12px; font-size: 9pt; vertical-align: middle; }}
        .checked {{ background-color: black; }}
    </style>
</head>
<body>
 <div class=""header"" style=""display: flex; justify-content: space-between; align-items: center;"">
    <img src='https://tulicenciapr.com/upload/iconoSupIzquierdoEvaluacion.png' class=""header-image"" alt=""Logo izquierdo"" style=""width: 100px; height: 70px; width: 110px"" />
    <p class=""s1"" style=""text-align: center; flex-grow: 1;"">CERTIFICACIÓN MÉDICA PARA CERTIFICADO DE LICENCIA DE CONDUCIR</p>
    <img src='https://tulicenciapr.com/upload/iconoSupDerechoEvaluacion.png' class=""header-image"" alt=""Logo derecho"" style=""width: 100px; height: 70px; width: 80px"" />
</div>
    <table style=""border-collapse:collapse; width:100%;"">
  
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>

     

    <tr style=""height:25pt"">
         
           <td style=""width:25%;""><p class=""s2"" style=""padding-left: 5pt; text-align: left;"">Nombre</p></td>
        <td style=""width:25%;""><p class=""underline"" style=""text-align: left;"">{evaluacion.NombreCliente }</p></td>
       
        
      
        </tr>       

        <tr style=""height:25pt"">
         
             
             <td style=""width:25%;""><p class=""s2"" style=""padding-left: 5pt; text-align: left;"">Segundo Nombre</p></td>
             <td style=""width:25%;""><p class=""underline"" style=""text-align: left;"">{evaluacion.SegundoNombreCliente}</p></td>

            
     
        </tr>
 <tr style=""height:25pt"">
         <td style=""width:25%;""><p class=""s2"" style=""padding-left: 5pt; text-align: left;"">Apellido Paterno</p></td>
       <td style=""width:25%;""><p class=""underline"" style=""text-align: left;"">{evaluacion.ApellidoPaternoCliente}</p></td>
        </tr>       

        <tr style=""height:25pt"">
         <td style=""width:25%;""><p class=""s2"" style=""padding-left: 5pt; text-align: left;"">Apellido Materno</p></td>
       <td style=""width:25%;""><p class=""underline"" style=""text-align: left;"">{evaluacion.ApellidoMaternoCliente}</p></td>
        </tr>



        <tr style=""height:30pt"">
            <td colspan=""3""><p class=""s2"" style=""padding-left: 5pt;"">Núm. de Seguro Social <u>{evaluacion.SeguroSocial}</u></p></td>
            <td colspan=""3""><p class=""s2"" style=""padding-left: 5pt; text-align:left;"">Núm. de Licencia de Conducir <u>{evaluacion.LicenciaConducir}</u></p></td>
        </tr>
        <tr style=""height:22pt"">
            <td colspan=""8""><p class=""s4"" style=""padding-top: 5pt; padding-left: 5pt; text-align:left;"">INSTRUCCIONES AL MÉDICO</p></td>
        </tr>
        <tr style=""height:50pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt; padding-right: 5pt;"">De acuerdo con las disposiciones de la Ley Núm. 22, de 7 de enero de 2000, según enmendada, conocida como “Ley de Vehículos y Tránsito de Puerto Rico”, todo aspirante a obtener Certificado de Licencia de Conducir Vehículos de Motor debe estar físicamente capacitado y sin aparente incapacidad mental para conducir. El médico examinará al solicitante personalmente y hará constar en este formulario las condiciones físicas en que se encuentre dicho solicitante.</p></td>
        </tr>
        <tr style=""height:22pt"">
            <td colspan=""8""><p class=""s4"" style=""padding-top: 3pt; padding-left: 5pt;"">Agudeza Visual</p></td>
        </tr>
        <tr style=""height:22pt"">
            <td style=""width:25%;"" colspan=""4""><p class=""s3"" style=""padding-left: 17pt;"">Ojo derecho sin lentes Correctivos 20/ <u>{evaluacion.OjoDerechoSinLentes}</u></p></td>
            <td style=""width:25%;"" colspan=""4""><p class=""s3"" style=""padding-left: 17pt;"">Ojo izquierdo sin lentes Correctivos 20/ <u>{evaluacion.OjoIzquierdoSinLentes}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td style=""width:25%;"" colspan=""4""><p class=""s3"" style=""padding-left: 17pt;"">Ojo derecho con lentes Correctivos 20/ <u>{evaluacion.OjoDerechoConLentes}</u></p></td>
            <td style=""width:25%;"" colspan=""4""><p class=""s3"" style=""padding-left: 17pt;"">Ojo izquierdo con lentes Correctivos 20/ <u>{evaluacion.OjoIzquierdoConLentes}</u></p></td>
        </tr>
        <tr style=""height:20pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 17pt;"">Ambos Ojos 20/ <u>{evaluacion.AmbosOjos}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""2""><p class=""s3"" style=""padding-left: 5pt;"">¿Usa espejuelos?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Espejuelos == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Espejuelos == "No" ? "checked" : "")}""></div> No</td>
            <td colspan=""2""></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""2""><p class=""s3"" style=""padding-left: 5pt;"">¿Usa lentes de contactos?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.UsaLentes == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.UsaLentes == "No" ? "checked" : "")}""></div> No</td>
            <td colspan=""2""></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Observaciones: <u>{evaluacion.Observacion}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Oídos: <u>{evaluacion.CondicionOido}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Brazos: <u>{evaluacion.CondicionBrazo}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Piernas: <u>{evaluacion.CondicionPierna}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Comentarios sobre condición física o mental del solicitante: <u>{evaluacion.CondicionFisica}</u></p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""4""><p class=""s3"" style=""padding-left: 5pt;"">¿Ha padecido alguna vez de epilepsia, mareos, etc.?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.EstadoInconciencia == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.EstadoInconciencia == "No" ? "checked" : "")}""></div> No</td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""4""><p class=""s3"" style=""padding-left: 5pt;"">¿Padece del corazón?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.PadeceCorazon == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.PadeceCorazon == "No" ? "checked" : "")}""></div> No</td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""4""><p class=""s3"" style=""padding-left: 5pt;"">¿Usa marcapaso?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Marcapaso == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Marcapaso == "No" ? "checked" : "")}""></div> No</td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""4""><p class=""s3"" style=""padding-left: 5pt;"">¿Usa prótesis?</p></td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Protesis == "Si" ? "checked" : "")}""></div> Sí</td>
            <td colspan=""2""><div class=""checkbox {(evaluacion.Protesis == "No" ? "checked" : "")}""></div> No</td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Peso <u>{evaluacion.Peso}</u> Libras</p></td>
        </tr>
        <tr style=""height:25pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">Estatura <u>{evaluacion.EstaturaPies}</u> Pies <u>{evaluacion.EstaturaPulgadas}</u> Pulgadas</p></td>
        </tr>
        <tr style=""height:80pt"">
            <td colspan=""8""><p class=""s3"" style=""padding-left: 5pt;"">El que suscribe, certifica que está debidamente autorizado a ejercer la profesión médica en Puerto Rico y hace constar que ha examinado a <u>{evaluacion.NombreCliente}  {evaluacion.SegundoNombreCliente}  {evaluacion.ApellidoPaternoCliente}  {evaluacion.ApellidoMaternoCliente}</u> y certifica que dicha persona <u>{(evaluacion.Estado == "1" ? "está físicamente y mentalmente capacitada" : "no está físicamente y mentalmente capacitada")}</u> para manejar vehículos de motor.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>


        <tr style=""height:80pt"">
            <td colspan=""2""><p class=""s3"" style=""padding-left: 5pt;"">Fecha del examen: <u>{evaluacion.FechaEvaluacion.ToString("dd/MM/yyyy")}</u></p></td>
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt;"">Firma del solicitante: </p></td>
        </tr>

<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>
<tr style=""height:80pt"">           
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt; color:white"">.</p></td>
        </tr>


        <tr style=""height:80pt"">
           <td colspan=""2""><p class=""s3"" style=""padding-left: 5pt;"">Firma del médico: </p></td>
             <td colspan=""3""><p class=""s3"" style=""padding-left: 0pt;"">Nombre y licencia del médico: <u>{evaluacion.NombreMedico}</u></p></td>
        </tr>
  
    </table>
    <p style=""padding-top: 6pt; padding-left: 5pt;"">Rev. 24ago2017</p>
</body>
</html>";

                    string rutaArchivoPDF = Path.Combine("wwwroot/Evaluaciones/", nombreArchivo);
                    GenerarPDFDesdeHTML(contenidoHTML, rutaArchivoPDF);

                    Console.WriteLine($"PDF generado exitosamente en: {rutaArchivoPDF}");
                    response.success = true;
                    response.message = "PDF generado correctamente";
                }
                else
                {
                    response.success = false;
                    response.message = "No se pudo obtener la evaluación";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = $"Error al generar el PDF: {ex.Message}";
            }

            return response;
        }
        private string AdjustStringSize(string input, int size)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new string('_', size);
            }

            if (input.Length < size)
            {
                return input.PadRight(size, '_');
            }

            return input;
        }
        private void GenerarPDFDesdeHTML(string htmlContent, string rutaArchivo)
        {
            using (var writer = new FileStream(rutaArchivo, FileMode.Create))
            {
                var pdfWriter = new PdfWriter(writer);
                var pdf = new PdfDocument(pdfWriter);
                ConverterProperties properties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlContent, pdf, properties);
            }
        }


        public async Task<ResponseEntity<Pago>> GetEstadoByPago(string pg_codigo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@pg_codigo", pg_codigo }
            };

            var response = await GetAllDataById<Pago>("sp_ObtenerEstadosForm", dr =>
            {
                return new Pago()
                {
                    pg_status = dr["pg_status"].ToString()
                };
            }, parameters);
            return response;
        }

        public async Task<Pago> GetPagoPorTransId(string transId)
        {
            var param = new Dictionary<string, object>() { { "@trans_id", transId } };
            var result = await GetData("sp_ObtenerPagoPorTransId", param, (dr) =>
            {
                return new Pago()
                {
                    pg_id = dr.IsDBNull(dr.GetOrdinal("pg_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("pg_id")),
                    cl_cliente = new Cliente
                    {
                        cl_id = dr.IsDBNull(dr.GetOrdinal("cl_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("cl_id")),
                        cl_nombreCompleto = dr.IsDBNull(dr.GetOrdinal("cl_nombreCompleto")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_nombreCompleto")),
                    },
                    tr_tramite = new Tramite
                    {
                        tr_id = dr.IsDBNull(dr.GetOrdinal("tr_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("tr_id")),
                        tr_nombre = dr.IsDBNull(dr.GetOrdinal("tr_nombre")) ? string.Empty : dr.GetString(dr.GetOrdinal("tr_nombre")),
                        tr_precio = dr.IsDBNull(dr.GetOrdinal("tr_precio")) ? 0 : Convert.ToDecimal(dr.GetDouble(dr.GetOrdinal("tr_precio"))),
                        tipoparcialtotal = dr.IsDBNull(dr.GetOrdinal("tipo_pago")) ? string.Empty : dr.GetString(dr.GetOrdinal("tipo_pago"))
                    },
                    pg_status = dr.IsDBNull(dr.GetOrdinal("pg_status")) ? string.Empty : dr.GetString(dr.GetOrdinal("pg_status")),
                    pg_codigo = dr.IsDBNull(dr.GetOrdinal("pg_codigo")) ? string.Empty : dr.GetString(dr.GetOrdinal("pg_codigo")),
                    pg_nota = dr.IsDBNull(dr.GetOrdinal("pg_nota")) ? string.Empty : dr.GetString(dr.GetOrdinal("pg_nota")),
                    pg_txid = dr.IsDBNull(dr.GetOrdinal("pg_txid")) ? string.Empty : dr.GetString(dr.GetOrdinal("pg_txid")),
                    pg_estado = dr.IsDBNull(dr.GetOrdinal("pg_estado")) ? 0 : dr.GetInt32(dr.GetOrdinal("pg_estado")),
                    pg_metodo = dr.IsDBNull(dr.GetOrdinal("pg_metodo")) ? 0 : dr.GetInt32(dr.GetOrdinal("pg_metodo")),
                    pg_fecha = dr.IsDBNull(dr.GetOrdinal("pg_fecha")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("pg_fecha")),
                    Monto = dr.IsDBNull(dr.GetOrdinal("tr_precio")) ? 0 : Convert.ToDecimal(dr.GetDouble(dr.GetOrdinal("tr_precio"))),
                    Description = dr.IsDBNull(dr.GetOrdinal("tr_nombre")) ? string.Empty : dr.GetString(dr.GetOrdinal("tr_nombre"))
                };
            });

            return result.item;
        }
    }
}
