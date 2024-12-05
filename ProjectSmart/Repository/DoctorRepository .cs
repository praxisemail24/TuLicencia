using System.Data;
using System.Data.SqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Controllers;
using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public class DoctorRepository : BaseRepository<CasoAsignado>, IDoctorRepository
    {
        private readonly IConfiguration _configuration;

        public DoctorRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
             
        public List<CasoAsignado> ObtenerCasosPorDoctorYFiltros(FiltroCasosRequest filtro)
        {
            return ExecProcedure<SqlConnection, SqlCommand, List<CasoAsignado>>("sp_ObtenerCasosPorDoctorYFiltros", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@DoctorId", filtro.DoctorId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@FechaDesde", filtro.FechaDesde ?? (object)DBNull.Value));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@FechaHasta", filtro.FechaHasta ?? (object)DBNull.Value));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@NombreTramite", string.IsNullOrEmpty(filtro.NombreTramite) ? "" : filtro.NombreTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@NombreCliente", string.IsNullOrEmpty(filtro.NombreCliente) ? "" : filtro.NombreCliente));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@Estado", string.IsNullOrEmpty(filtro.Estado) ? "" : filtro.Estado));
                var reader = cmd.ExecuteReader();
                var list = new List<CasoAsignado>();
                while (reader.Read())
                {
                    var item = new CasoAsignado();
                    item.RowIndex = reader.IsDBNull(reader.GetOrdinal("rowIndex")) ? -1 : reader.GetInt64(reader.GetOrdinal("rowIndex"));
                    item.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id"));
                    item.tr_id = reader.IsDBNull(reader.GetOrdinal("tr_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tr_id"));
                    item.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombreCliente")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreCliente"));
                    item.NombreTramite = reader.IsDBNull(reader.GetOrdinal("nombreTramite")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreTramite"));
                    item.TipoTramite = reader.IsDBNull(reader.GetOrdinal("tipoTramite")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipoTramite"));
                    item.Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado"));
                    item.Codigo = reader.IsDBNull(reader.GetOrdinal("pg_codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("pg_codigo"));
                    item.Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fecha"));
                    list.Add(item);
                }
                return list;
            });
        }

        public async Task GuardarEvaluacion(EvaluacionRequest request)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@Id", request.Id },
                    { "@tr_id", request.tr_id },
                    { "@OjoDerechoSinLentes", request.OjoDerechoSinLentes },
                    { "@OjoIzquierdoSinLentes", request.OjoIzquierdoSinLentes },
                    { "@OjoDerechoConLentes", request.OjoDerechoConLentes },
                    { "@OjoIzquierdoConLentes", request.OjoIzquierdoConLentes },
                    { "@Estado", request.Estado },
                    { "@FechaEvaluacion", request.FechaEvaluacion },
                    { "@NombreMedico", request.NombreMedico },
                    { "@LicenciaMedico", request.LicenciaMedico },
                    { "@Condicion", request.Condicion },
                    { "@AmbosOjos", request.AmbosOjos },
                    { "@Espejuelos", request.Espejuelos },
                    { "@UsaLentes", request.UsaLentes },
                    { "@Observacion", request.Observacion },
                    { "@CondicionOido", request.CondicionOido },
                    { "@CondicionBrazo", request.CondicionBrazo },
                    { "@CondicionPierna", request.CondicionPierna },
                    { "@CondicionFisica", request.CondicionFisica },
                    { "@EstadoInconciencia", request.EstadoInconciencia },
                    { "@PadeceCorazon", request.PadeceCorazon },
                    { "@Marcapaso", request.Marcapaso },
                    { "@Protesis", request.Protesis },
                    { "@Peso", request.Peso },
                    { "@ColorOjo", request.ColorOjo },
                    { "@ColorPelo", request.ColorPelo },
                    { "@EstaturaPies", request.EstaturaPies },
                    { "@EstaturaPulgadas", request.EstaturaPulgadas },
                    { "@CondisionConLentes", request.CondisionConLentes },
                    { "@CondisionSinLentes", request.CondisionSinLentes }

                    //,
                    //{ "@NombreCliente", request.NombreCliente },
                    //{ "@SegundoNombreCliente", request.SegundoNombreCliente },
                    //{ "@ApellidoPaternoCliente", request.ApellidoPaternoCliente },
                    //{ "@ApellidoMaternoCliente", request.ApellidoMaternoCliente },
                    //{ "@SeguroSocial", request.SeguroSocial },
                    //{ "@LicenciaConducir", request.LicenciaConducir }
                };

                await ExecuteNonQueryAsync("sp_GuardarEvaluacion", parameters);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
                throw;
            }
        }

        public async Task<EvaluacionRequest> ObtenerEvaluacionPorId(int id,int tr_id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
            {
                { "@Id", id },
        { "@tr_id", tr_id }
    };

                var response = await GetSingleDataById("sp_ObtenerEvaluacionPorId", (SqlDataReader dr) =>
                {
                    return new EvaluacionRequest()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        OjoDerechoSinLentes = dr.IsDBNull(dr.GetOrdinal("OjoDerechoSinLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("OjoDerechoSinLentes")),
                        OjoIzquierdoSinLentes = dr.IsDBNull(dr.GetOrdinal("OjoIzquierdoSinLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("OjoIzquierdoSinLentes")),
                        OjoDerechoConLentes = dr.IsDBNull(dr.GetOrdinal("OjoDerechoConLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("OjoDerechoConLentes")),
                        OjoIzquierdoConLentes = dr.IsDBNull(dr.GetOrdinal("OjoIzquierdoConLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("OjoIzquierdoConLentes")),
                        Estado = dr.IsDBNull(dr.GetOrdinal("Estado")) ? string.Empty : dr.GetString(dr.GetOrdinal("Estado")),
                        FechaEvaluacion = dr.IsDBNull(dr.GetOrdinal("FechaEvaluacion")) ? DateTime.MinValue : Convert.ToDateTime(dr["FechaEvaluacion"]),
                        NombreMedico = dr.IsDBNull(dr.GetOrdinal("NombreMedico")) ? string.Empty : dr.GetString(dr.GetOrdinal("NombreMedico")),
                        LicenciaMedico = dr.IsDBNull(dr.GetOrdinal("LicenciaMedico")) ? string.Empty : dr.GetString(dr.GetOrdinal("LicenciaMedico")),
                        NombreCliente = dr.IsDBNull(dr.GetOrdinal("NombreCliente")) ? string.Empty : dr.GetString(dr.GetOrdinal("NombreCliente")),
                        SegundoNombreCliente = dr.IsDBNull(dr.GetOrdinal("SegundoNombreCliente")) ? string.Empty : dr.GetString(dr.GetOrdinal("SegundoNombreCliente")),
                        ApellidoPaternoCliente = dr.IsDBNull(dr.GetOrdinal("ApellidoPaternoCliente")) ? string.Empty : dr.GetString(dr.GetOrdinal("ApellidoPaternoCliente")),
                        ApellidoMaternoCliente = dr.IsDBNull(dr.GetOrdinal("ApellidoMaternoCliente")) ? string.Empty : dr.GetString(dr.GetOrdinal("ApellidoMaternoCliente")),
                        SeguroSocial = dr.IsDBNull(dr.GetOrdinal("SeguroSocial")) ? string.Empty : dr.GetString(dr.GetOrdinal("SeguroSocial")),
                        LicenciaConducir = dr.IsDBNull(dr.GetOrdinal("LicenciaConducir")) ? string.Empty : dr.GetString(dr.GetOrdinal("LicenciaConducir")),

                        Condicion = dr.IsDBNull(dr.GetOrdinal("Condicion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Condicion")),
                        CondisionConLentes = dr.IsDBNull(dr.GetOrdinal("CondisionConLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondisionConLentes")),
                        CondisionSinLentes = dr.IsDBNull(dr.GetOrdinal("CondisionSinLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondisionSinLentes")),
                        AmbosOjos = dr.IsDBNull(dr.GetOrdinal("AmbosOjos")) ? string.Empty : dr.GetString(dr.GetOrdinal("AmbosOjos")),
                        Espejuelos = dr.IsDBNull(dr.GetOrdinal("Espejuelos")) ? string.Empty : dr.GetString(dr.GetOrdinal("Espejuelos")),
                        UsaLentes = dr.IsDBNull(dr.GetOrdinal("UsaLentes")) ? string.Empty : dr.GetString(dr.GetOrdinal("UsaLentes")),
                        Observacion = dr.IsDBNull(dr.GetOrdinal("Observacion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Observacion")),
                        CondicionOido = dr.IsDBNull(dr.GetOrdinal("CondicionOido")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondicionOido")),
                        CondicionBrazo = dr.IsDBNull(dr.GetOrdinal("CondicionBrazo")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondicionBrazo")),
                        CondicionPierna = dr.IsDBNull(dr.GetOrdinal("CondicionPierna")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondicionPierna")),
                        CondicionFisica = dr.IsDBNull(dr.GetOrdinal("CondicionFisica")) ? string.Empty : dr.GetString(dr.GetOrdinal("CondicionFisica")),
                        EstadoInconciencia = dr.IsDBNull(dr.GetOrdinal("EstadoInconciencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("EstadoInconciencia")),
                        PadeceCorazon = dr.IsDBNull(dr.GetOrdinal("PadeceCorazon")) ? string.Empty : dr.GetString(dr.GetOrdinal("PadeceCorazon")),
                        Marcapaso = dr.IsDBNull(dr.GetOrdinal("Marcapaso")) ? string.Empty : dr.GetString(dr.GetOrdinal("Marcapaso")),
                        Protesis = dr.IsDBNull(dr.GetOrdinal("Protesis")) ? string.Empty : dr.GetString(dr.GetOrdinal("Protesis")),
                        Peso = dr.IsDBNull(dr.GetOrdinal("Peso")) ? string.Empty : dr.GetString(dr.GetOrdinal("Peso")),
                        ColorOjo = dr.IsDBNull(dr.GetOrdinal("ColorOjo")) ? string.Empty : dr.GetString(dr.GetOrdinal("ColorOjo")),
                        ColorPelo = dr.IsDBNull(dr.GetOrdinal("ColorPelo")) ? string.Empty : dr.GetString(dr.GetOrdinal("ColorPelo")),
                        EstaturaPies = dr.IsDBNull(dr.GetOrdinal("EstaturaPies")) ? string.Empty : dr.GetString(dr.GetOrdinal("EstaturaPies")),
                        EstaturaPulgadas = dr.IsDBNull(dr.GetOrdinal("EstaturaPulgadas")) ? string.Empty : dr.GetString(dr.GetOrdinal("EstaturaPulgadas"))


                    };
                }, parameters);

                return response;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
                throw;
            }
        }

        private async Task ExecuteNonQueryAsync(string storedProcedure, Dictionary<string, object> parameters)
        {
            string conexion = _configuration.GetConnectionString("Conexion");

            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                    }
                }
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        // Implementación de GetSingleDataById
        private async Task<T> GetSingleDataById<T>(string storedProcedure, Func<SqlDataReader, T> mapFunction, Dictionary<string, object> parameters = null)
        {
            T result = default(T);
            string conexion = _configuration.GetConnectionString("Conexion");

            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = mapFunction(reader);
                    }
                }
            }
            return result;
        }


        // Otros métodos específicos de DoctorRepository si son necesarios
    }
}
