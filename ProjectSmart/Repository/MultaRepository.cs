using SmartLicencia.Data;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class MultaRepository : AbstractConnection
    {
        public MultaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Multa> Listar(int TramiteId, int FormularioId)
        {
            try
            {

            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<Multa>>("sp_ListarMultas", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", FormularioId));
                var reader = cmd.ExecuteReader();
                var lista = new List<Multa>();
                while (reader.Read())
                {
                    var multa = new Multa();
                    multa.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    multa.TramiteId = reader.GetInt32(reader.GetOrdinal("TRAMITE_ID"));
                    multa.FormularioId = reader.GetInt32(reader.GetOrdinal("FORMULARIO_ID"));
                    multa.ClienteId = reader.GetInt32(reader.GetOrdinal("CLIENTE_ID"));
                    multa.Origen = reader.IsDBNull(reader.GetOrdinal("ORIGEN")) ? string.Empty : reader.GetString(reader.GetOrdinal("ORIGEN"));
                    multa.Descripcion = reader.IsDBNull(reader.GetOrdinal("DESCRIPCION")) ? string.Empty : reader.GetString(reader.GetOrdinal("DESCRIPCION"));
                    multa.Monto = reader.IsDBNull(reader.GetOrdinal("MONTO")) ? 0.0m : reader.GetDecimal(reader.GetOrdinal("MONTO"));
                    multa.Estado = reader.IsDBNull(reader.GetOrdinal("ESTADO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ESTADO"));
                    multa.AutorId = reader.GetInt32(reader.GetOrdinal("AUTOR_ID"));
                    multa.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"));
                    multa.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("ULTIMA_ACTUALIZACION")) ? null : reader.GetDateTime(reader.GetOrdinal("ULTIMA_ACTUALIZACION"));
                    lista.Add(multa);
                }
                return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<Multa>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<Multa>();
            }
        }

        public IEnumerable<MultaArchivo> ListarArchivos(int TramiteId, int FormularioId)
        {
            try
            {

            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<MultaArchivo>>("sp_ListarMultaArchivos", (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", FormularioId));
                var reader = cmd.ExecuteReader();
                var lista = new List<MultaArchivo>();
                while (reader.Read())
                {
                    var archivo = new MultaArchivo();
                    archivo.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    archivo.TramiteId = reader.GetInt32(reader.GetOrdinal("TRAMITE_ID"));
                    archivo.FormularioId = reader.GetInt32(reader.GetOrdinal("FORMULARIO_ID"));
                    archivo.NombreArchivo = reader.IsDBNull(reader.GetOrdinal("NOMBRE_ARCHIVO")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOMBRE_ARCHIVO"));
                    archivo.RootPath = reader.IsDBNull(reader.GetOrdinal("ROOT_PATH")) ? string.Empty : reader.GetString(reader.GetOrdinal("ROOT_PATH"));
                    archivo.Url = reader.IsDBNull(reader.GetOrdinal("URL")) ? string.Empty : reader.GetString(reader.GetOrdinal("URL"));
                    archivo.MimeType = reader.IsDBNull(reader.GetOrdinal("MIME_TYPE")) ? string.Empty : reader.GetString(reader.GetOrdinal("MIME_TYPE"));
                    archivo.Origen = reader.IsDBNull(reader.GetOrdinal("ORIGEN")) ? string.Empty : reader.GetString(reader.GetOrdinal("ORIGEN"));
                    archivo.Tamanio = reader.IsDBNull(reader.GetOrdinal("TAMANIO")) ? 0 : reader.GetInt64(reader.GetOrdinal("TAMANIO"));
                    archivo.AutorId = reader.GetInt32(reader.GetOrdinal("AUTOR_ID"));
                    archivo.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"));
                    archivo.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("ULTIMA_ACTUALIZACION")) ? null : reader.GetDateTime(reader.GetOrdinal("ULTIMA_ACTUALIZACION"));
                    lista.Add(archivo);
                }
                return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<MultaArchivo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<MultaArchivo>();
            }
        }

        public IEnumerable<MultaCuota> ListarCuotas(int id)
        {
            try
            {
            string query = "SELECT * FROM MultaCuota WHERE MULTA_PAGO_ID = @id";
            return ExecQuery<SqlConnection, SqlCommand, IEnumerable<MultaCuota>>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                var reader = cmd.ExecuteReader();
                var lista = new List<MultaCuota>();
                while (reader.Read())
                {
                    var cuota = new MultaCuota();
                    cuota.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                    cuota.MultaPagoId = reader.GetInt32(reader.GetOrdinal("MULTA_PAGO_ID"));
                    cuota.NroCuota = reader.GetInt32(reader.GetOrdinal("NRO_CUOTA"));
                    cuota.Monto = reader.GetDecimal(reader.GetOrdinal("MONTO"));
                    cuota.Fecha = reader.IsDBNull(reader.GetOrdinal("FECHA")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA"));
                    cuota.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"));
                    cuota.UltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("ULTIMA_ACTUALIZACION")) ? null : reader.GetDateTime(reader.GetOrdinal("ULTIMA_ACTUALIZACION"));
                    lista.Add(cuota);
                }
                return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<MultaCuota>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<MultaCuota>();
            }

        }

        public IEnumerable<MultaPagoEntity> ListarPagos(MultaPagoRequest filter)
        {
            try
            {
                return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<MultaPagoEntity>>("sp_ListarMultaPagos", (cmd) =>
                {
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipoTramite", filter.TipoTramite));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@nombreCliente", filter.NombreCliente));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@correo", filter.Correo));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@telefono", filter.Telefono));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha", filter.Fecha));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", filter.Estado));
                    var reader = cmd.ExecuteReader();
                    List<MultaPagoEntity> lista = new List<MultaPagoEntity>();
                    while (reader.Read())
                    {
                        var pago = new MultaPagoEntity();
                        pago.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        pago.TramiteId = reader.GetInt32(reader.GetOrdinal("TRAMITE_ID"));
                        pago.FormularioId = reader.GetInt32(reader.GetOrdinal("FORMULARIO_ID"));
                        pago.AutorId = reader.GetInt32(reader.GetOrdinal("AUTOR_ID"));
                        pago.Total = reader.IsDBNull(reader.GetOrdinal("TOTAL")) ? 0m : reader.GetDecimal(reader.GetOrdinal("TOTAL"));
                        pago.CantidadCuotas = reader.IsDBNull(reader.GetOrdinal("CANTIDAD_CUOTAS")) ? 0 : reader.GetInt32(reader.GetOrdinal("CANTIDAD_CUOTAS"));
                        pago.Origen = reader.IsDBNull(reader.GetOrdinal("ORIGEN")) ? string.Empty : reader.GetString(reader.GetOrdinal("ORIGEN"));
                        pago.Tipo = reader.IsDBNull(reader.GetOrdinal("TIPO")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIPO"));
                        pago.CodigoPago = reader.IsDBNull(reader.GetOrdinal("CODIGO_PAGO")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_PAGO"));
                        pago.Pagado = reader.IsDBNull(reader.GetOrdinal("PAGADO")) ? false : reader.GetBoolean(reader.GetOrdinal("PAGADO"));
                        pago.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"));
                        pago.UltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("ULTIMA_ACTUALIZACION")) ? null : reader.GetDateTime(reader.GetOrdinal("ULTIMA_ACTUALIZACION"));
                        lista.Add(pago);
                    }
                    return lista;
                });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<MultaPagoEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<MultaPagoEntity>();
            }
        }

        public MultaPago? ObtenerPago(int TramiteId, int FormularioId)
        {
            string query = "SELECT TOP 1 * FROM MultaPago WHERE TRAMITE_ID = @trId AND FORMULARIO_ID = @frmId";
            return ExecQuery<SqlConnection, SqlCommand, MultaPago?>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", FormularioId));
                var reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    var pago = new MultaPago();
                    pago.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    pago.TramiteId = reader.GetInt32(reader.GetOrdinal("TRAMITE_ID"));
                    pago.FormularioId = reader.GetInt32(reader.GetOrdinal("FORMULARIO_ID"));
                    pago.AutorId = reader.GetInt32(reader.GetOrdinal("AUTOR_ID"));
                    pago.Total = reader.IsDBNull(reader.GetOrdinal("TOTAL")) ? 0m : reader.GetDecimal(reader.GetOrdinal("TOTAL"));
                    pago.CantidadCuotas = reader.IsDBNull(reader.GetOrdinal("CANTIDAD_CUOTAS")) ? 0 : reader.GetInt32(reader.GetOrdinal("CANTIDAD_CUOTAS"));
                    pago.Origen = reader.IsDBNull(reader.GetOrdinal("ORIGEN")) ? string.Empty : reader.GetString(reader.GetOrdinal("ORIGEN"));
                    pago.Tipo = reader.IsDBNull(reader.GetOrdinal("TIPO")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIPO"));
                    pago.CodigoPago = reader.IsDBNull(reader.GetOrdinal("CODIGO_PAGO")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_PAGO"));
                    pago.Pagado = reader.IsDBNull(reader.GetOrdinal("PAGADO")) ? false : reader.GetBoolean(reader.GetOrdinal("PAGADO"));
                    pago.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FECHA_CREACION")) ? null : reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"));
                    pago.UltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("ULTIMA_ACTUALIZACION")) ? null : reader.GetDateTime(reader.GetOrdinal("ULTIMA_ACTUALIZACION"));
                    return pago;
                }
                return null;
            });
        }

        public bool GuardarMulta(Multa multa)
        {
            string queryStr = string.Empty;

            if(multa.Id == 0)
            {
                queryStr = "INSERT INTO Multa (TRAMITE_ID, FORMULARIO_ID, CLIENTE_ID, ORIGEN, DESCRIPCION, MONTO, ESTADO, AUTOR_ID, FECHA_CREACION, ULTIMA_ACTUALIZACION)";
                queryStr += " VALUES (@tramiteId, @formularioId, @clienteId, @origen, @descripcion, @monto, @estado, @autorId, GETDATE(), GETDATE())";
            } else
            {
                queryStr = "UPDATE Multa SET TRAMITE_ID = @tramiteId, FORMULARIO_ID = @formularioId, CLIENTE_ID = @clienteId, ORIGEN = @origen, DESCRIPCION = @descripcion,";
                queryStr += " MONTO = @monto, ESTADO = @estado, AUTOR_ID = @autorId, ULTIMA_ACTUALIZACION = GETDATE() WHERE ID = @id";
            }

            return ExecQuery<SqlConnection, SqlCommand, bool>(queryStr, (cmd) =>
            {
                if(multa.Id > 0)
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", multa.Id));

                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tramiteId", multa.TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@formularioId", multa.FormularioId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clienteId", multa.ClienteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@origen", multa.Origen));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@descripcion", multa.Descripcion));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@monto", multa.Monto));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", multa.Estado));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@autorId", multa.AutorId));
                return cmd.ExecuteNonQuery() > 0;
            });
        }

        public bool BorraMulta(Multa multa)
        {
            string queryStr = string.Empty;

           
                queryStr = "Delete Multa  WHERE ID = @id";
           

            return ExecQuery<SqlConnection, SqlCommand, bool>(queryStr, (cmd) =>
            {
                //if (multa.Id > 0)
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", multa.Id));

                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@tramiteId", multa.TramiteId));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@formularioId", multa.FormularioId));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@clienteId", multa.ClienteId));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@origen", multa.Origen));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@descripcion", multa.Descripcion));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@monto", multa.Monto));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", multa.Estado));
                //cmd.Parameters.Add(CreateParameter<SqlParameter>("@autorId", multa.AutorId));
                return cmd.ExecuteNonQuery() > 0;
            });
        }

        public bool GuardarArchivo(MultaArchivo archivo)
        {
            string queryStr = string.Empty;

            if (archivo.Id == 0)
            {
                queryStr = "INSERT INTO MultaArchivo (NOMBRE_ARCHIVO, ROOT_PATH, URL, MIME_TYPE, TAMANIO, MULTA_ID, AUTOR_ID, TRAMITE_ID, FORMULARIO_ID, ORIGEN, FECHA_CREACION, ULTIMA_ACTUALIZACION)";
                queryStr += " VALUES (@nombreArchivo, @path, @url, @mimeType, @tamanio, @multaId, @autorId, @tramiteId, @formularioId, @origen, GETDATE(), GETDATE());";
            } else
            {
                queryStr = "UPDATE MultaArchivo SET NOMBRE_ARCHIVO = @nombreArchivo, ROOT_PATH = @path, URL = @url, TAMANIO = @tamanio, MULTA_ID = @multaId, MIME_TYPE = @mimeType, ORIGEN = @origen,";
                queryStr += " AUTOR_ID = @autorId, TRAMITE_ID = @tramiteId, FORMULARIO_ID = @formularioId, ULTIMA_ACTUALIZACION = GETDATE() WHERE ID = @id";
            }

            return ExecQuery<SqlConnection, SqlCommand, bool>(queryStr, (cmd) =>
            {
                if (archivo.Id > 0)
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", archivo.Id));

                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tramiteId", archivo.TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@formularioId", archivo.FormularioId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nombreArchivo", archivo.NombreArchivo));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@path", archivo.RootPath));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@url", archivo.Url));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tamanio", archivo.Tamanio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@multaId", archivo.MultaId == null ? DBNull.Value : archivo.MultaId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@autorId", archivo.AutorId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@mimeType", archivo.MimeType));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@origen", archivo.Origen));
                return cmd.ExecuteNonQuery() > 0;
            });
        }

        public int GuardarPago(MultaPago pago)
        {
            string queryStr = string.Empty;

            if(pago.Id == 0)
            {
                queryStr = "INSERT INTO MultaPago (TOTAL, FORMULARIO_ID, TRAMITE_ID, CLIENTE_ID, CANTIDAD_CUOTAS, AUTOR_ID, ORIGEN, TIPO, PAGADO, FECHA_CREACION, ULTIMA_ACTUALIZACION)";
                queryStr += " VALUES(@total, @formularioId, @tramiteId, @clienteId, @cantCuotas, @autorId, @origen, @tipo, @pagado, GETDATE(), GETDATE());";
            } else
            {
                queryStr = "UPDATE MultaPago SET TOTAL=@total, FORMULARIO_ID=@formularioId, TRAMITE_ID=@tramiteId, CLIENTE_ID=@clienteId, CANTIDAD_CUOTAS=@cantCuotas, AUTOR_ID=@autorId,";
                queryStr += "ULTIMA_ACTUALIZACION=GETDATE(), ORIGEN=@origen, TIPO=@tipo, PAGADO=@pagado WHERE ID = @id";
            }

            return ExecQuery<SqlConnection, SqlCommand, int>(queryStr, (cmd) =>
            {
                if (pago.Id > 0)
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", pago.Id));

                cmd.Parameters.Add(CreateParameter<SqlParameter>("@formularioId", pago.FormularioId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tramiteId", pago.TramiteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clienteId", pago.ClienteId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@total", pago.Total));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@cantCuotas", pago.CantidadCuotas));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@autorId", pago.AutorId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@origen", pago.Origen));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo", pago.Tipo));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pagado", pago.Pagado));
                var rows = cmd.ExecuteNonQuery();

                if(rows > 0)
                {
                    if(pago.Id == 0)
                    {
                        var cmdId = CreateCommand<SqlCommand>();
                        cmdId.CommandText = "SELECT MAX(ID) as LastId FROM MultaPago";
                        cmdId.CommandType = System.Data.CommandType.Text;
                        cmdId.Connection = cmd.Connection;
                        int id = Convert.ToInt32(cmdId.ExecuteScalar());
                        cmdId.Dispose();

                        return id;
                    } else
                    {
                        return pago.Id;
                    }
                }

                return 0;
            });
        }

        public bool GuardarCuota(MultaCuota cuota)
        {
            string queryStr = string.Empty;

            if (cuota.Id == 0)
            {
                queryStr = "INSERT INTO MultaCuota (MULTA_PAGO_ID, NRO_CUOTA, FECHA, MONTO, ESTADO, FECHA_CREACION, ULTIMA_ACTUALIZACION)";
                queryStr += " VALUES (@pagoId, @nroCuota, @fecha, @monto, @estado, GETDATE(), GETDATE())";
            }
            else
            {
                queryStr = "UPDATE MultaCuota SET MULTA_PAGO_ID = @pagoId,";
                queryStr += " NRO_CUOTA = @nroCuota, FECHA = @fecha, MONTO = @monto, ULTIMA_ACTUALIZACION = GETDATE(), ESTADO = @estado WHERE ID = @id";
            }

            return ExecQuery<SqlConnection, SqlCommand, bool>(queryStr, (cmd) =>
            {
                if (cuota.Id > 0)
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", cuota.Id));

                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nroCuota", cuota.NroCuota));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha", cuota.Fecha));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@monto", cuota.Monto));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pagoId", cuota.MultaPagoId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", cuota.Estado));
                return cmd.ExecuteNonQuery() > 0;
            });
        }

        public string ObtenerPagoCesco(int tr_id, int frm_id, int cl_id)
        {
            string query = "SELECT count(*) AS X FROM MultaPago WHERE origen='cesco' and TRAMITE_ID = @trId AND FORMULARIO_ID = @frmId and CLIENTE_ID = @clId";
            return ExecQuery<SqlConnection, SqlCommand, string?>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", tr_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", frm_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clId", cl_id));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int count = reader.GetInt32(reader.GetOrdinal("X"));
                        return count.ToString();
                    }
                }

                return null;
            });
        }

        public string ObtenerPagoCescoTipo(int tr_id, int frm_id, int cl_id)
        {
            string query = "SELECT TIPO X FROM MultaPago WHERE  TRAMITE_ID = @trId AND FORMULARIO_ID = @frmId and CLIENTE_ID = @clId";
            return ExecQuery<SqlConnection, SqlCommand, string?>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", tr_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", frm_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clId", cl_id));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string count = reader.GetString(reader.GetOrdinal("X"));
                        return count.ToString();
                    }
                }

                return null;
            });
        }

        

        public string ObtenerPagoAutoExpress(int tr_id, int frm_id, int cl_id)
        {
            string query = "SELECT count(*) AS X FROM MultaPago WHERE origen='AutoExpress' and TRAMITE_ID = @trId AND FORMULARIO_ID = @frmId and CLIENTE_ID = @clId";
            return ExecQuery<SqlConnection, SqlCommand, string?>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", tr_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", frm_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clId", cl_id));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int count = reader.GetInt32(reader.GetOrdinal("X"));
                        return count.ToString();
                    }
                }

                return null;
            });
        }
    }
}
