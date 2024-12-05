using SmartLicencia.Data;
using SmartLicense.Pdf.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class CertificadoRepository : AbstractConnection
    {
        public CertificadoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public CertificatePdf ObtenerCertificado(int trId, int frmId)
        {
            return ExecProcedure<SqlConnection, SqlCommand, CertificatePdf>("sp_ObtenerCertificadoPdf", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tr_id", trId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frm_id", frmId));
                var reader = cmd.ExecuteReader();
                var pdf = new CertificatePdf();
                if (reader.Read())
                {
                    if(!reader.IsDBNull(reader.GetOrdinal("fecha_nacimiento")))
                    {
                        var fechaNac = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"));
                        pdf.FechaNacDia = fechaNac.Day.ToString();
                        pdf.FechaNacMes = fechaNac.Month.ToString();
                        pdf.FechaNacAnio = fechaNac.Year.ToString();
                    }

                    pdf.Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("motivo"));
                    pdf.NroLicencia = reader.IsDBNull(reader.GetOrdinal("nro_licencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("nro_licencia"));
                    pdf.Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("categoria"));
                    pdf.VehiculoPesado = reader.IsDBNull(reader.GetOrdinal("vehiculo_pesado")) ? string.Empty : reader.GetString(reader.GetOrdinal("vehiculo_pesado"));
                    pdf.Identificacion = reader.IsDBNull(reader.GetOrdinal("identificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("identificacion"));
                    pdf.EstadoLegal = reader.IsDBNull(reader.GetOrdinal("estado_legal")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado_legal"));
                    pdf.PrimerNombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre"));
                    pdf.SegundoNombre = reader.IsDBNull(reader.GetOrdinal("nombre2")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre2"));
                    pdf.ApellidoPaterno = reader.IsDBNull(reader.GetOrdinal("ape_paterno")) ? string.Empty : reader.GetString(reader.GetOrdinal("ape_paterno"));
                    pdf.ApellidoMaterno = reader.IsDBNull(reader.GetOrdinal("ape_materno")) ? string.Empty : reader.GetString(reader.GetOrdinal("ape_materno"));
                    pdf.ApellidoMaterno = reader.IsDBNull(reader.GetOrdinal("ape_materno")) ? string.Empty : reader.GetString(reader.GetOrdinal("ape_materno"));
                    pdf.Numero = reader.IsDBNull(reader.GetOrdinal("numero")) ? string.Empty : reader.GetString(reader.GetOrdinal("numero"));
                    pdf.Donante = reader.IsDBNull(reader.GetOrdinal("donante")) ? string.Empty : reader.GetString(reader.GetOrdinal("donante"));
                    pdf.TipoSangre = reader.IsDBNull(reader.GetOrdinal("tipo_sangre")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_sangre"));
                    pdf.Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? string.Empty : reader.GetString(reader.GetOrdinal("genero"));
                    pdf.Peso = reader.IsDBNull(reader.GetOrdinal("peso")) ? string.Empty : reader.GetString(reader.GetOrdinal("peso"));
                    pdf.NroTelefono = reader.IsDBNull(reader.GetOrdinal("nro_telefono")) ? string.Empty : reader.GetString(reader.GetOrdinal("nro_telefono"));
                    pdf.Tez = reader.IsDBNull(reader.GetOrdinal("tez")) ? string.Empty : reader.GetString(reader.GetOrdinal("tez"));
                    pdf.Pelo = reader.IsDBNull(reader.GetOrdinal("color_pelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("color_pelo"));
                    pdf.Ojos = reader.IsDBNull(reader.GetOrdinal("color_ojos")) ? string.Empty : reader.GetString(reader.GetOrdinal("color_ojos"));
                    pdf.DirReferencialUrbanicacion = reader.IsDBNull(reader.GetOrdinal("residencial_urbanizacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("residencial_urbanizacion"));
                    pdf.DirReferencialCalle = reader.IsDBNull(reader.GetOrdinal("residencial_calle")) ? string.Empty : reader.GetString(reader.GetOrdinal("residencial_calle"));
                    pdf.DirReferencialPueblo = reader.IsDBNull(reader.GetOrdinal("residencial_pueblo")) ? string.Empty : reader.GetString(reader.GetOrdinal("residencial_pueblo"));
                    pdf.DirReferencialCodPostal = reader.IsDBNull(reader.GetOrdinal("residencial_cod_postal")) ? string.Empty : reader.GetString(reader.GetOrdinal("residencial_cod_postal"));
                    pdf.DirPostalBarrio = reader.IsDBNull(reader.GetOrdinal("postal_barrio")) ? string.Empty : reader.GetString(reader.GetOrdinal("postal_barrio"));
                    pdf.DirPostalPueblo = reader.IsDBNull(reader.GetOrdinal("postal_pueblo")) ? string.Empty : reader.GetString(reader.GetOrdinal("postal_pueblo"));
                    pdf.DirPostalCodPostal = reader.IsDBNull(reader.GetOrdinal("postal_codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("postal_codigo"));
                    pdf.LicSuspendida = reader.IsDBNull(reader.GetOrdinal("suspension")) ? string.Empty : reader.GetString(reader.GetOrdinal("suspension"));
                    pdf.SuspensionTipo = reader.IsDBNull(reader.GetOrdinal("suspension_tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("suspension_tipo"));
                    pdf.Respuesta1 = reader.IsDBNull(reader.GetOrdinal("recluido")) ? string.Empty : reader.GetString(reader.GetOrdinal("recluido"));
                    pdf.Respuesta2 = reader.IsDBNull(reader.GetOrdinal("convicto_bebidas")) ? string.Empty : reader.GetString(reader.GetOrdinal("convicto_bebidas"));
                    pdf.Respuesta2Fecha = reader.IsDBNull(reader.GetOrdinal("convicto_bebidas_fecha")) ? string.Empty : reader.GetString(reader.GetOrdinal("convicto_bebidas_fecha"));
                    pdf.Respuesta3 = reader.IsDBNull(reader.GetOrdinal("convicto_narcoticos")) ? string.Empty : reader.GetString(reader.GetOrdinal("convicto_narcoticos"));
                    pdf.Respuesta3Fecha = reader.IsDBNull(reader.GetOrdinal("convicto_narcoticos_fecha")) ? string.Empty : reader.GetString(reader.GetOrdinal("convicto_narcoticos_fecha"));
                    pdf.Respuesta4 = reader.IsDBNull(reader.GetOrdinal("asume")) ? string.Empty : reader.GetString(reader.GetOrdinal("asume"));
                    pdf.Respuesta5 = reader.IsDBNull(reader.GetOrdinal("acaa")) ? string.Empty : reader.GetString(reader.GetOrdinal("acaa"));
                    pdf.PaisProcede = reader.IsDBNull(reader.GetOrdinal("pais_procede")) ? string.Empty : reader.GetString(reader.GetOrdinal("pais_procede"));
                    pdf.EstadoProcede = reader.IsDBNull(reader.GetOrdinal("estado_procede")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado_procede"));
                    pdf.Var1 = reader.IsDBNull(reader.GetOrdinal("var1")) ? string.Empty : reader.GetString(reader.GetOrdinal("var1"));
                    pdf.Var2 = reader.IsDBNull(reader.GetOrdinal("var2")) ? string.Empty : reader.GetString(reader.GetOrdinal("var2"));

                    if (!reader.IsDBNull(reader.GetOrdinal("talla")))
                    {
                        var partials = reader.GetString(reader.GetOrdinal("talla")).Split("'");
                        
                        if(partials.Length == 2)
                        {
                            pdf.EstaturaMtrs = partials[0];
                            pdf.EstaturaCm = partials[1].Replace('"', ' ');
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("fecha_reg")))
                        pdf.FechaReg = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", reader.GetDateTime(reader.GetOrdinal("fecha_reg")));

                    if (!reader.IsDBNull(reader.GetOrdinal("fecha_fin")))
                        pdf.FechaFin = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", reader.GetDateTime(reader.GetOrdinal("fecha_fin")));
                }
                return pdf;
            });
        }

        public List<ItemImagen> ObtenerArchivos(int trId, int? pgId)
        {
            //return ExecProcedure<SqlConnection, SqlCommand, List<ItemImagen>>("sp_ListarArchivosPorPagoId", (cmd) =>
            //return ExecProcedure<SqlConnection, SqlCommand, List<ItemImagen>>("sp_ObtenerArchivosPor_Pg_Id", (cmd) =>
            var stringStore = "";

            switch (trId)
            {
                case 1:
                    stringStore = "sp_ObtenerArchivosReno_Pg_Id";
                    break;
                case 3:
                    stringStore = "sp_ObtenerArchivosDupli_Pg_Id";
                    break;
                case 4:
                    stringStore = "sp_ObtenerArchivosRecip_Pg_Id";
                    break;
                case 5:
                    stringStore = "sp_ObtenerArchivosTraspaso_Pg_Id";
                    break;
                default:
                    break;
            }

            return ExecProcedure<SqlConnection, SqlCommand, List<ItemImagen>>(stringStore, (cmd) =>
            {
                //cmd.Parameters.AddWithValue("@tr_id", trId);
                cmd.Parameters.AddWithValue("@pg_id", (pgId == null ? 0 : pgId));

                var reader = cmd.ExecuteReader();
                var pdf = new ImagePdf();
                List<ItemImagen> items = new List<ItemImagen>();
                while (reader.Read())
                {
                    var index = reader.GetInt32(reader.GetOrdinal("ar_pos"));
                    var nombre = reader.GetString(reader.GetOrdinal("ar_nombre"));
                    //var frmId = reader.GetInt32(reader.GetOrdinal("frm_id"));
                    var key = ImagePdf.TituloImg(trId, index);
                    if (!string.IsNullOrWhiteSpace(key) && !items.Exists(x => x.Nombre == key))
                    {
                        items.Add(new ItemImagen
                        {
                            Nombre = key,
                            Position = index,
                            FrmId = 0,
                            Url = nombre,
                        });
                    }
                }
                return items;
            });
        }


        public int? Obtener_pgId(int trId, int frmId)
        {
            var tableMapping = new Dictionary<int, (string Table, string Column)>
            {
                { 1, ("Frm_RenovacionLicencia", "frl_id") },
                { 3, ("Frm_DuplicadoLicencia", "fdl_id") },
                { 4, ("Frm_LicenciaReciprocidad", "flr_id") },
                { 5, ("Frm_Traspaso", "ID") },
            };

            if (!tableMapping.TryGetValue(trId, out var tableInfo))
            {
                throw new ArgumentException("El trId proporcionado no es válido.");
            }

            string queryDef = $"SELECT pg_id FROM {tableInfo.Table} WHERE {tableInfo.Column} = {frmId}";
     
            return ExecQuery<SqlConnection, SqlCommand, int?>(queryDef, (cmd) =>
            {
                cmd.Parameters.AddWithValue("@frmId", frmId);

                using var reader = cmd.ExecuteReader();
            
                if (reader.Read())
                {
                    return reader.GetInt32(0); 
                }

                return null; 
            });
        }


        public ImagePdf ObtenerFirma(int trId, int? pgId)
        {
            string query = $"SELECT * FROM Archivos WHERE tr_id = {trId} AND pg_id = {pgId} AND  (ar_pos in (30))";
            return ExecQuery<SqlConnection, SqlCommand, ImagePdf>(query, (cmd) =>
            {
                var reader = cmd.ExecuteReader();
                var pdf = new ImagePdf();
                while (reader.Read())
                {
                    var index = reader.GetInt32(reader.GetOrdinal("ar_pos"));
                    var nombre = reader.GetString(reader.GetOrdinal("ar_nombre"));
                    var key = ImagePdf.TituloImg(trId, index);
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        if (pdf.Imagenes.Keys.ToList().Contains(key))
                            pdf.Imagenes[key] = nombre;
                        else
                            pdf.Imagenes.Add(key, nombre);
                    }
                }
                return pdf;
            });
        }

        public ImagePdf ObtenerFirmaDoctor(int trId, int frmId)
        {
            string query = $"select Firma ar_nombre, 100 ar_pos from (select fdl_id frm_id,Doctor,tr_id from Frm_DuplicadoLicencia union select fla_id,Doctor,tr_id from Frm_LicenciaAprendizaje union select frl_id,Doctor,tr_id from Frm_RenovacionLicencia )x inner join Administrador a on x.doctor=a.adm_id WHERE tr_id = {trId} AND frm_id = {frmId} ";
            return ExecQuery<SqlConnection, SqlCommand, ImagePdf>(query, (cmd) =>
            {
                var reader = cmd.ExecuteReader();
                var pdf = new ImagePdf();
                while (reader.Read())
                {
                    var index = reader.GetInt32(reader.GetOrdinal("ar_pos"));
                    var nombre = reader.GetString(reader.GetOrdinal("ar_nombre"));
                    var key = ImagePdf.TituloImg(trId, index);
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        if (pdf.Imagenes.Keys.ToList().Contains(key))
                            pdf.Imagenes[key] = nombre;
                        else
                            pdf.Imagenes.Add(key, nombre);
                    }
                }
                return pdf;
            });
        }

        public string GetAutorizacion(int trId, int frmId, int? pgId, string _rootDirectory)
        {
            string query = $"SELECT ar_nombre FROM Archivos WHERE tr_id = {trId} AND pg_id = {pgId} AND ar_pos = 20";
            return ExecQuery<SqlConnection, SqlCommand, string>(query, (cmd) =>
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return DownloadFile( reader.GetString(reader.GetOrdinal("ar_nombre")),_rootDirectory+"\\"+"certiAutorizacion_"+trId+"_"+frmId+".pdf" ) ;
                }
                return null;
            });
        }

        public string DownloadFile(string url, string localPath)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(url).Result;  // Usar .Result para obtener el resultado de forma síncrona
                response.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    response.Content.CopyToAsync(fileStream).Wait();  // Usar .Wait() para esperar la tarea de forma síncrona
                }
            }
            return localPath;
        }


        public MedicalCertificatePdf CertificadoMedico(int trId, int frmId)
        {
            return ExecProcedure<SqlConnection, SqlCommand, MedicalCertificatePdf>("sp_ObtenerCerficadoMedicoPdf", (cmd) =>
            {
                cmd.Parameters.AddWithValue("@tr_id", trId);
                cmd.Parameters.AddWithValue("@frm_id", frmId);
                var reader = cmd.ExecuteReader();
                MedicalCertificatePdf certificado = new MedicalCertificatePdf();
                if (reader.Read())
                {
                    certificado.FrmId = reader.GetInt32(reader.GetOrdinal("id"));
                    certificado.TrId = reader.GetInt32(reader.GetOrdinal("trId"));
                    certificado.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombreCliente")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreCliente"));
                    certificado.SegundoNombreCliente = reader.IsDBNull(reader.GetOrdinal("segundoNombreCliente")) ? string.Empty : reader.GetString(reader.GetOrdinal("segundoNombreCliente"));
                    certificado.ApellidoPaternoCliente = reader.IsDBNull(reader.GetOrdinal("apellidoPaterno")) ? string.Empty : reader.GetString(reader.GetOrdinal("apellidoPaterno"));
                    certificado.ApellidoMaternoCliente = reader.IsDBNull(reader.GetOrdinal("apellidoMaterno")) ? string.Empty : reader.GetString(reader.GetOrdinal("apellidoMaterno"));
                    certificado.SeguroSocial = reader.IsDBNull(reader.GetOrdinal("seguroSocial")) ? string.Empty : reader.GetString(reader.GetOrdinal("seguroSocial"));
                    certificado.LicenciaConducir = reader.IsDBNull(reader.GetOrdinal("licenciaConducir")) ? string.Empty : reader.GetString(reader.GetOrdinal("licenciaConducir"));
                    certificado.OjoDerechoSinLentes = reader.IsDBNull(reader.GetOrdinal("ojoDerechoSinLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("ojoDerechoSinLentes"));
                    certificado.OjoIzquierdoSinLentes = reader.IsDBNull(reader.GetOrdinal("ojoIzquierdoSinLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("ojoIzquierdoSinLentes"));
                    certificado.OjoDerechoConLentes = reader.IsDBNull(reader.GetOrdinal("ojoDerechoConLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("ojoDerechoConLentes"));
                    certificado.OjoIzquierdoConLentes = reader.IsDBNull(reader.GetOrdinal("ojoIzquierdoConLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("ojoIzquierdoConLentes"));
                    certificado.AmbosOjos = reader.IsDBNull(reader.GetOrdinal("ambosOjos")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambosOjos"));
                    certificado.Espejuelos = reader.IsDBNull(reader.GetOrdinal("espejuelos")) ? string.Empty : reader.GetString(reader.GetOrdinal("espejuelos"));
                    certificado.Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado"));
                    certificado.CondicionBrazo = reader.IsDBNull(reader.GetOrdinal("condicionBrazo")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionBrazo"));
                    certificado.CondicionOido = reader.IsDBNull(reader.GetOrdinal("condicionOido")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionOido"));
                    certificado.CondicionFisica = reader.IsDBNull(reader.GetOrdinal("condicionFisica")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionFisica"));
                    certificado.CondicionPierna = reader.IsDBNull(reader.GetOrdinal("condicionPierna")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionPierna"));
                    certificado.EstadoInconciencia = reader.IsDBNull(reader.GetOrdinal("estadoInconciencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoInconciencia"));
                    certificado.EstaturaPulgadas = reader.IsDBNull(reader.GetOrdinal("estaturaPulgadas")) ? string.Empty : reader.GetString(reader.GetOrdinal("estaturaPulgadas"));
                    certificado.EstaturaPies = reader.IsDBNull(reader.GetOrdinal("estaturaPies")) ? string.Empty : reader.GetString(reader.GetOrdinal("estaturaPies"));
                    certificado.FechaEvaluacion = reader.IsDBNull(reader.GetOrdinal("fechaEvaluacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("fechaEvaluacion"));
                    certificado.Observacion = reader.IsDBNull(reader.GetOrdinal("observacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("observacion"));
                    certificado.Marcapaso = reader.IsDBNull(reader.GetOrdinal("marcapaso")) ? string.Empty : reader.GetString(reader.GetOrdinal("marcapaso"));
                    certificado.NombreMedico = reader.IsDBNull(reader.GetOrdinal("nombreMedico")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreMedico"));
                    certificado.PadeceCorazon = reader.IsDBNull(reader.GetOrdinal("padeceCorazon")) ? string.Empty : reader.GetString(reader.GetOrdinal("padeceCorazon"));
                    certificado.Peso = reader.IsDBNull(reader.GetOrdinal("peso")) ? string.Empty : reader.GetString(reader.GetOrdinal("peso"));
                    certificado.Protesis = reader.IsDBNull(reader.GetOrdinal("protesis")) ? string.Empty : reader.GetString(reader.GetOrdinal("protesis"));
                    certificado.UsaLentes = reader.IsDBNull(reader.GetOrdinal("usaLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("usaLentes"));
                    certificado.ColorOjos = reader.IsDBNull(reader.GetOrdinal("colorOjo")) ? string.Empty : reader.GetString(reader.GetOrdinal("colorOjo"));
                    certificado.ColorPelo = reader.IsDBNull(reader.GetOrdinal("colorPelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("colorPelo"));
                    certificado.CondicionConLentes = reader.IsDBNull(reader.GetOrdinal("condicionConLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionConLentes"));
                    certificado.CondicionSinLentes = reader.IsDBNull(reader.GetOrdinal("condicionSinLentes")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionSinLentes"));
                }
                reader.Close();
                return certificado;
            });
        }

    }
}
