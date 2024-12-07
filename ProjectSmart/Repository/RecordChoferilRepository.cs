using SmartLicencia.Data;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class RecordChoferilRepository : AbstractConnection
    {
        public RecordChoferilRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public RecordChoferil Store(RecordChoferil request)
        {
            return ExecProcedure<SqlConnection, SqlCommand, RecordChoferil>("sp_GuardarRecordChoferil", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", request.Id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@lang", request.Lang));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@type", request.CertifiedType));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@licensePlate", request.LicensePlate));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@number", request.Number));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@serie", request.Serie));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@category", request.Category));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pgId", request.PgId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clId", request.ClId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@expeditionAt", request.ExpeditionAt));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@expirationAt", request.ExpirationAt));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@purposeApplication", request.PurposeApplication));

                request.Id = Convert.ToInt64(cmd.ExecuteScalar());

                return request;
            });
        }

        public RecordChoferil GetById(long id)
        {
            return ExecProcedure<SqlConnection, SqlCommand, RecordChoferil>("sp_ObtenerRecordChoferil", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                var reader = cmd.ExecuteReader();

                var record = new RecordChoferil();

                if(reader.Read())
                {
                    record.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                    record.ClId = reader.IsDBNull(reader.GetOrdinal("CL_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CL_ID"));
                    record.PgId = reader.IsDBNull(reader.GetOrdinal("PG_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PG_ID"));
                    record.Lang = reader.IsDBNull(reader.GetOrdinal("LANG")) ? string.Empty : reader.GetString(reader.GetOrdinal("LANG"));
                    record.LicensePlate = reader.IsDBNull(reader.GetOrdinal("LICENSE_PLATE")) ? string.Empty : reader.GetString(reader.GetOrdinal("LICENSE_PLATE"));
                }

                return record;
            });
        }
    }
}
