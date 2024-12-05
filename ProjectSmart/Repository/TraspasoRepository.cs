using SmartLicencia.Data;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class TraspasoRepository : AbstractConnection
    {
        public TraspasoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Traspaso Store(Traspaso model)
        {
            if (model == null) throw new ArgumentNullException("Se requiere infromación de traspaso.");

            return ExecProcedure<SqlConnection, SqlCommand, Traspaso>("sp_GuardarTraspaso", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", model.Id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@seller_id", model.Seller.cl_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@buyer_id", model.Buyer.cl_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pg_id", model.PgId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tpr_id", model.TprId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tr_id", model.TrId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@brand", model.BrandVehicle));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@model", model.ModelVehicle));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@year", model.YearVehicle));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@color", model.ColorVehicle));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@serie", model.SerialNumber));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@other_info", model.OtherInfo));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@transfer_type", model.TransferType));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@contract_date", model.ContractDate));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@has_contract", model.HasContract));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@currency", model.Currency));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@payment_date", model.PaymentDate));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@payment_amount", model.PaymentAmount));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@license_plate", model.LicensePlate));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@author_id", model.AuthorId));

                if (model.Id == 0)
                {
                    model.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    if(model.Id == 0)
                        throw new Exception("Error al intentar registrar los datos del traspaso.");
                }
                else
                {
                    var rows = cmd.ExecuteNonQuery();

                    if (rows <= 0)
                        throw new Exception("Error al intentar actualizar los datos del traspaso.");
                }

                return model;
            });
        }

        public Traspaso GetById(int id)
        {
            var model = ExecProcedure<SqlConnection, SqlCommand, Traspaso?>("sp_ObtenerTraspasoPorId", (cmd) =>
            {
                Traspaso? traspaso = null;
                cmd.Parameters.Add(CreateParameter<SqlParameter>("id", id));
                var reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    traspaso = new Traspaso();
                    traspaso.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                    traspaso.PgId = reader.IsDBNull(reader.GetOrdinal("PG_ID")) ? null : reader.GetInt32(reader.GetOrdinal("PG_ID"));
                    traspaso.TprId = reader.IsDBNull(reader.GetOrdinal("TPR_ID")) ? null : reader.GetInt32(reader.GetOrdinal("TPR_ID"));
                    traspaso.TrId = reader.IsDBNull(reader.GetOrdinal("TR_ID")) ? null : reader.GetInt32(reader.GetOrdinal("TR_ID"));
                    traspaso.Currency = reader.IsDBNull(reader.GetOrdinal("CURRENCY")) ? "USD" : reader.GetString(reader.GetOrdinal("CURRENCY"));
                    traspaso.BrandVehicle = reader.IsDBNull(reader.GetOrdinal("BRAND_VEHICLE")) ? string.Empty : reader.GetString(reader.GetOrdinal("BRAND_VEHICLE"));
                    traspaso.ModelVehicle = reader.IsDBNull(reader.GetOrdinal("MODEL_VEHICLE")) ? string.Empty : reader.GetString(reader.GetOrdinal("MODEL_VEHICLE"));
                    traspaso.ColorVehicle = reader.IsDBNull(reader.GetOrdinal("COLOR_VEHICLE")) ? string.Empty : reader.GetString(reader.GetOrdinal("COLOR_VEHICLE"));
                    traspaso.LicensePlate = reader.IsDBNull(reader.GetOrdinal("LICENSE_PLATE")) ? string.Empty : reader.GetString(reader.GetOrdinal("LICENSE_PLATE"));
                    traspaso.YearVehicle = reader.IsDBNull(reader.GetOrdinal("YEAR_VEHICLE")) ? null : reader.GetInt32(reader.GetOrdinal("YEAR_VEHICLE"));
                    traspaso.SerialNumber = reader.IsDBNull(reader.GetOrdinal("SERIAL_NUMBER")) ? string.Empty : reader.GetString(reader.GetOrdinal("SERIAL_NUMBER"));
                    traspaso.OtherInfo = reader.IsDBNull(reader.GetOrdinal("OTHER_INFO")) ? string.Empty : reader.GetString(reader.GetOrdinal("OTHER_INFO"));
                    traspaso.TransferType = reader.IsDBNull(reader.GetOrdinal("TRANSFER_TYPE")) ? string.Empty : reader.GetString(reader.GetOrdinal("TRANSFER_TYPE"));
                    traspaso.ContractDate = reader.IsDBNull(reader.GetOrdinal("CONTRACT_DATE")) ? null : reader.GetDateTime(reader.GetOrdinal("CONTRACT_DATE"));
                    traspaso.HasContract = reader.IsDBNull(reader.GetOrdinal("HAS_CONTRACT")) ? false : reader.GetBoolean(reader.GetOrdinal("HAS_CONTRACT"));
                    traspaso.PaymentDate = reader.IsDBNull(reader.GetOrdinal("PAYMENT_DATE")) ? null : reader.GetDateTime(reader.GetOrdinal("PAYMENT_DATE"));
                    traspaso.PaymentAmount = reader.IsDBNull(reader.GetOrdinal("PAYMENT_AMOUNT")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PAYMENT_AMOUNT"));
                    traspaso.State = reader.IsDBNull(reader.GetOrdinal("STATE")) ? 0 : reader.GetInt32(reader.GetOrdinal("STATE"));
                    traspaso.RevisedStatus = reader.IsDBNull(reader.GetOrdinal("REVISED_STATUS")) ? null : reader.GetInt32(reader.GetOrdinal("REVISED_STATUS")) == 1;
                    traspaso.RevisedStatusAt = reader.IsDBNull(reader.GetOrdinal("REVISED_STATUS_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("REVISED_STATUS_AT"));
                    traspaso.EvaluationStatus = reader.IsDBNull(reader.GetOrdinal("EVALUATION_STATUS")) ? null : reader.GetBoolean(reader.GetOrdinal("EVALUATION_STATUS"));
                    traspaso.EvaluationStatusAt = reader.IsDBNull(reader.GetOrdinal("EVALUATION_STATUS_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("EVALUATION_STATUS_AT"));
                    traspaso.RadicatorId = reader.IsDBNull(reader.GetOrdinal("RADICATOR_ID")) ? null : reader.GetInt32(reader.GetOrdinal("RADICATOR_ID"));
                    traspaso.AdminId = reader.IsDBNull(reader.GetOrdinal("ADMIN_ID")) ? null : reader.GetInt32(reader.GetOrdinal("ADMIN_ID"));
                    traspaso.RadicatorAsignatedAt = reader.IsDBNull(reader.GetOrdinal("RADICATOR_ASIGNATED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("RADICATOR_ASIGNATED_AT"));
                    traspaso.RadicatedStatusAt = reader.IsDBNull(reader.GetOrdinal("RADICATED_STATUS_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("RADICATED_STATUS_AT"));
                    traspaso.RadicatedStatus = reader.IsDBNull(reader.GetOrdinal("RADICATED_STATUS")) ? null : reader.GetBoolean(reader.GetOrdinal("RADICATED_STATUS"));
                    traspaso.RadicationState = reader.IsDBNull(reader.GetOrdinal("RADICATION_STATE")) ? 0 : reader.GetInt32(reader.GetOrdinal("RADICATION_STATE"));
                    traspaso.RadicationObservation = reader.IsDBNull(reader.GetOrdinal("RADICATION_OBSERVATION")) ? string.Empty : reader.GetString(reader.GetOrdinal("RADICATION_OBSERVATION"));
                    traspaso.AdminName = reader.IsDBNull(reader.GetOrdinal("ADMIN_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("ADMIN_NAME"));
                    traspaso.RadicatorName = reader.IsDBNull(reader.GetOrdinal("RADICATOR_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("RADICATOR_NAME"));

                    traspaso.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CREATED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("CREATED_AT"));
                    traspaso.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UPDATED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("UPDATED_AT"));
                    traspaso.ClosedAt = reader.IsDBNull(reader.GetOrdinal("CLOSED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("CLOSED_AT"));
                    traspaso.ProcessedAt = reader.IsDBNull(reader.GetOrdinal("PROCESSED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("PROCESSED_AT"));
                    traspaso.ReviewedAt = reader.IsDBNull(reader.GetOrdinal("REVIEWED_AT")) ? null : reader.GetDateTime(reader.GetOrdinal("REVIEWED_AT"));
                    traspaso.AuthorId = reader.IsDBNull(reader.GetOrdinal("CL_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CL_ID"));

                    traspaso.Seller.cl_id = reader.IsDBNull(reader.GetOrdinal("SELLER_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SELLER_ID"));
                    traspaso.Seller.cl_nombre = reader.IsDBNull(reader.GetOrdinal("SELLER_FIRST_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_FIRST_NAME")).TrimStart().TrimEnd();
                    traspaso.Seller.cl_segundoNombre = reader.IsDBNull(reader.GetOrdinal("SELLER_MIDDLE_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_MIDDLE_NAME")).TrimStart().TrimEnd();
                    traspaso.Seller.cl_primerApellido = reader.IsDBNull(reader.GetOrdinal("SELLER_FIRST_SURNAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_FIRST_SURNAME")).TrimStart().TrimEnd();
                    traspaso.Seller.cl_segundoApellido = reader.IsDBNull(reader.GetOrdinal("SELLER_MIDDLE_SURNAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_MIDDLE_SURNAME")).TrimStart().TrimEnd();
                    traspaso.Seller.cl_zip = reader.IsDBNull(reader.GetOrdinal("SELLER_ZIP")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_ZIP"));
                    traspaso.Seller.cl_correo = reader.IsDBNull(reader.GetOrdinal("SELLER_EMAIL")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_EMAIL"));
                    traspaso.Seller.cl_direccion = reader.IsDBNull(reader.GetOrdinal("SELLER_ADDRESS")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_ADDRESS"));
                    traspaso.Seller.cl_numeroLicencia = reader.IsDBNull(reader.GetOrdinal("SELLER_NROLICENSE")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_NROLICENSE"));
                    traspaso.Seller.cl_numeroSeguro = reader.IsDBNull(reader.GetOrdinal("SELLER_SAFENUMBER")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_SAFENUMBER"));
                    traspaso.Seller.cl_numeroTelefono = reader.IsDBNull(reader.GetOrdinal("SELLER_PHONE")) ? string.Empty : reader.GetString(reader.GetOrdinal("SELLER_PHONE"));
                    var pblSeller = reader.GetString(reader.GetOrdinal("SELLER_PBL")).Split("-");
                    traspaso.Seller.cl_pueblo = new Pueblos
                    {
                        pl_id = Convert.ToInt32(pblSeller[0]),
                        pl_nombre = pblSeller[1],
                    };

                    traspaso.Buyer.cl_id = reader.IsDBNull(reader.GetOrdinal("BUYER_ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BUYER_ID"));
                    traspaso.Buyer.cl_nombre = reader.IsDBNull(reader.GetOrdinal("BUYER_FIRST_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_FIRST_NAME")).TrimStart().TrimEnd();
                    traspaso.Buyer.cl_segundoNombre = reader.IsDBNull(reader.GetOrdinal("BUYER_MIDDLE_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_MIDDLE_NAME")).TrimStart().TrimEnd();
                    traspaso.Buyer.cl_primerApellido = reader.IsDBNull(reader.GetOrdinal("BUYER_FIRST_SURNAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_FIRST_SURNAME")).TrimStart().TrimEnd();
                    traspaso.Buyer.cl_segundoApellido = reader.IsDBNull(reader.GetOrdinal("BUYER_MIDDLE_SURNAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_MIDDLE_SURNAME")).TrimStart().TrimEnd();
                    traspaso.Buyer.cl_zip = reader.IsDBNull(reader.GetOrdinal("BUYER_ZIP")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_ZIP"));
                    traspaso.Buyer.cl_correo = reader.IsDBNull(reader.GetOrdinal("BUYER_EMAIL")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_EMAIL"));
                    traspaso.Buyer.cl_direccion = reader.IsDBNull(reader.GetOrdinal("BUYER_ADDRESS")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_ADDRESS"));
                    traspaso.Buyer.cl_numeroLicencia = reader.IsDBNull(reader.GetOrdinal("BUYER_NROLICENSE")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_NROLICENSE"));
                    traspaso.Buyer.cl_numeroSeguro = reader.IsDBNull(reader.GetOrdinal("BUYER_SAFENUMBER")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_SAFENUMBER"));
                    traspaso.Buyer.cl_numeroTelefono = reader.IsDBNull(reader.GetOrdinal("BUYER_PHONE")) ? string.Empty : reader.GetString(reader.GetOrdinal("BUYER_PHONE"));
                    var pblBuyer = reader.GetString(reader.GetOrdinal("BUYER_PBL")).Split("-");
                    traspaso.Buyer.cl_pueblo = new Pueblos
                    {
                        pl_id = Convert.ToInt32(pblBuyer[0]),
                        pl_nombre = pblBuyer[1],
                    };

                    traspaso.Seller.cl_nombreCompleto = $"{traspaso.Seller.cl_nombre} {traspaso.Seller.cl_segundoNombre} {traspaso.Seller.cl_primerApellido} {traspaso.Seller.cl_segundoApellido}";
                    traspaso.Buyer.cl_nombreCompleto = $"{traspaso.Buyer.cl_nombre} {traspaso.Buyer.cl_segundoNombre} {traspaso.Buyer.cl_primerApellido} {traspaso.Buyer.cl_segundoApellido}";
                }
                return traspaso;
            });

            if (model == null) throw new Exception("Registro de traspaso no encontrado.");

            return model;
        }

        public List<TraspasoDoc> GetDocuments(int id)
        {
            return ExecProcedure<SqlConnection, SqlCommand, List<TraspasoDoc>>("sp_ListarArchivosTraspasoById", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));

                var docs = new List<TraspasoDoc>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var file = new TraspasoDoc();
                    file.ar_id = reader.GetInt32(reader.GetOrdinal("ar_id"));
                    file.frm_id = reader.GetInt32(reader.GetOrdinal("frm_id"));
                    file.pg_id = reader.IsDBNull(reader.GetOrdinal("pg_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("pg_id"));
                    file.ar_url = reader.IsDBNull(reader.GetOrdinal("ar_nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("ar_nombre"));
                    file.ar_fecha = reader.IsDBNull(reader.GetOrdinal("ar_fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ar_fecha"));
                    file.ar_pos = reader.IsDBNull(reader.GetOrdinal("ar_pos")) ? -1 : reader.GetInt32(reader.GetOrdinal("ar_pos"));
                    file.ar_estado = reader.IsDBNull(reader.GetOrdinal("ar_estado")) ? 0 : reader.GetInt32(reader.GetOrdinal("ar_estado"));
                    file.cl_cliente = new Cliente
                    {
                        cl_id = reader.IsDBNull(reader.GetOrdinal("cl_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("cl_id")),
                    };
                    file.tr_tramite = new Tramite
                    {
                        tr_id = reader.IsDBNull(reader.GetOrdinal("tr_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tr_id")),
                    };
                    file.ar_nombre = TraspasoDoc.GetFileName(file.ar_pos);
                    docs.Add(file);
                }
                return docs;
            });
        }

        public bool ChangeCase(long id, int state)
        {
            return ExecProcedure<SqlConnection, SqlCommand, bool>("sp_CambiarCasoTraspaso", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@state", state));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@date", DateTime.Now));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public bool ChangeStatus(long id, bool? revised, bool? evaluation, DateTime? revisedAt = null, DateTime? evaluationAt = null)
        {
            return ExecProcedure<SqlConnection, SqlCommand, bool>("sp_CambiarEstadoTraspaso", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@revised", revised));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@evaluation", evaluation));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@revisedAt", revisedAt));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@evaluationAt", evaluationAt));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public bool RadicatorAsigned(long id, int? adminId, int? radicatorId, DateTime? radicatorAsignedAt = null)
        {
            radicatorAsignedAt = radicatorAsignedAt ?? DateTime.Now;
            var query = $"UPDATE Frm_Traspaso SET ADMIN_ID = @adminId, RADICATOR_ID = @radicatorId, RADICATOR_ASIGNATED_AT = @asignatedAt, CREATED_AT = CURRENT_TIMESTAMP WHERE ID = @id";
            return ExecQuery<SqlConnection, SqlCommand, bool>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@adminId", adminId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@radicatorId", radicatorId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@asignatedAt", radicatorAsignedAt));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public bool Radicated(long id, bool? status, DateTime? radicatedAt = null)
        {
            radicatedAt = radicatedAt ?? DateTime.Now;
            var query = $"UPDATE Frm_Traspaso SET RADICATED_STATUS = @status, RADICATED_STATUS_AT = @radicatedAt, CREATED_AT = CURRENT_TIMESTAMP WHERE ID = @id";
            return ExecQuery<SqlConnection, SqlCommand, bool>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@status", status));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@radicatedAt", radicatedAt));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public bool RadicationStatus(long id, string? radicationState, string? radicationObservation)
        {
            var query = $"UPDATE Frm_Traspaso SET RADICATION_STATE = @radicationState, RADICATION_OBSERVATION = @radicationObservation, CREATED_AT = CURRENT_TIMESTAMP WHERE ID = @id";
            return ExecQuery<SqlConnection, SqlCommand, bool>(query, (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@radicationState", radicationState));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@radicationObservation", radicationObservation));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }
    }
}
