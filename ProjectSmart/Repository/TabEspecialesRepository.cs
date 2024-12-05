using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class TabEspecialesRepository : BaseRepository<TabEspeciales>, ITabEspecialesRepository
    {
        private readonly IConfiguration _configuration;

        public TabEspecialesRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        public async Task<ResponseEntity<TabEspeciales>> AddTabEspeciales(TabEspeciales tabEspeciales)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                {
                    { "frl_id", tabEspeciales.frl_id},
                    { "cl_id", tabEspeciales.cl_id},
                    { "pg_id", tabEspeciales.pg_id},
                    { "tr_id", tabEspeciales.tr_id },
                    { "fte_nombre", tabEspeciales.fte_nombre },
                    { "fte_primerApellido", tabEspeciales.fte_primerApellido },
                    { "fte_segundoApellido", tabEspeciales.fte_segundoApellido },
                    { "fte_identificacion", tabEspeciales.fte_identificacion },
                    { "fte_numero", tabEspeciales.fte_numero },
                    { "fte_numeroLicencia", tabEspeciales.fte_numeroLicencia },
                    { "fte_direccion", tabEspeciales.fte_direccion },
                    { "fte_numeroDireccion", tabEspeciales.fte_numeroDireccion },
                    { "fte_pueblo", tabEspeciales.fte_pueblo},
                    { "fte_codigoPostal", tabEspeciales.fte_codigoPostal},
                    { "fte_barrio", tabEspeciales.fte_barrio },
                    { "fte_apartado", tabEspeciales.fte_apartado},
                    { "fte_pueblo2", tabEspeciales.fte_pueblo2 },
                    { "fte_codigoPostal2", tabEspeciales.fte_codigoPostal2 },
                    { "fte_numTablilla", tabEspeciales.fte_numTablilla },
                    { "fte_numRegistro", tabEspeciales.fte_numRegistro },
                    { "fte_numbTiuloPropiedad", tabEspeciales.fte_numbTiuloPropiedad },
                    { "fte_marca", tabEspeciales.fte_marca },
                    { "fte_modelo", tabEspeciales.fte_modelo },
                    { "fte_anio", tabEspeciales.fte_anio },
                    { "fte_numSerie", tabEspeciales.fte_numSerie }
                };
                ResponseEntity<TabEspeciales> result = await AddAsync(tabEspeciales, "sp_RegistrarTabEspeciales", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<TabEspeciales> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<TabEspeciales> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
    }
}
