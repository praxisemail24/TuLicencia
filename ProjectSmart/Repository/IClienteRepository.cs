using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IClienteRepository
    {
        Task<ResponseEntity<Cliente>> GetAllCliente();
        Task<ResponseEntity<Cliente>> GetClienteById(int id);
        Task<ResponseEntity<Cliente>> GetClienteByKeyTemp(string cl_keyTemporal);
        Task<ResponseEntity<int>> GetFrlIdByClienteAndTramite(int cl_id, int tr_id, int pg_id);



        Task<ResponseEntity<string>> GetPagoByClienteAndTramite(int cl_id, int tr_id);
        Task<ResponseEntity<Cliente>> GetLoginbyUsuario(string cl_nombreUsuario, string cl_contrasena);
        Task<ResponseEntity<Cliente>> AddCliente(Cliente Cliente);
        Task<ResponseEntity<Cliente>> UpdateCliente(Cliente Cliente);
        Task<ResponseEntity<Cliente>> DeleteCliente(int id);
        Task<ResponseEntity<Cliente>> GetFrmValidacion(int cl_id, int pg_id, int tr_id);
        Task<ResponseEntity<Cliente>> UpdateClienteCambioPass(int cl_id, string newPass);
        Task<ResponseEntity<Cliente>> UpdateClientePassword(int cl_id, string newPass, string cl_keytemporal);
        Task<ResponseEntity<Cliente>> UpdateClienteKeyTemporal(int clienteId, string keyTemporal);
        Task<ResponseEntity<Cliente>> GetClienteByCorreo(string correo);
        Task<Cliente> GetClienteByCorreo2Async(string correo);
        Task<ResponseEntity<Dictionary<string, object>>> GetValidarArchivos(int tr_id, int cl_id, int frm_id);

        Task<ResponseEntity<Cliente>> UpdateClienteUsuario(int cl_id, string newUsuario, string cl_keytemporal);

        Task<ResponseEntity<int>> GetValidarEstadoForm(int cl_id, int tr_id);

        Task<ResponseEntity<Cliente>> GetBuscarClientePanel(string cl_nombre = null, string cl_primerApellido = null, string cl_segundoApellido = null, string cl_correo = null, string cl_nombreUsuario = null, string cl_numeroLicencia = null, string cl_numeroTelefono = null);
        Task<ResponseEntity<DetallePago>> GetDetallePago(string codigoPago);
        Task<IEnumerable<Cliente>> Search(string column, string search);
    }
}
