using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IEmailRepository
    {
        Task<(bool success, string errorMessage)> EmailNewPass(Email request);
        Task<(bool success, string errorMessage)> EmailNewUsuario(Email request);
        void EmailRegistro(Cliente cliente);
        void EmailConfirmacionFormulario(Cliente cliente, string nombreForm);
        void EmailConfirmacionArchivos(Cliente cliente);
        Task<ResponseEntity> EmailPago(Pago pago);
        ResponseEntity EmailPagoX(Pago pago);
        void EmailPagoError(string correo, string nombre, string apellido, string errores);
        void EmailConfirmacionCambioPass(Cliente cliente);

        void EmailPagoDiferido(string correo, string nombre, string apellido, List<Cuota> Cuota);

    }
}
