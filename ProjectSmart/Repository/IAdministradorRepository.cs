using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IAdministradorRepository
    {
        Task<ResponseEntity<Administrador>> GetAllAdministrador();
        Task<ResponseEntity<Administrador>> GetAdministradorById(int id);
        Task<ResponseEntity<Administrador>> GetLogin(string adm_nombres, string adm_clv);
        Task<ResponseEntity<Administrador>> GetLogin(LoginModel Login);

        Task<ResponseEntity<Administrador>> AddAdministrador(Administrador administrador);
        Task<ResponseEntity<Administrador>> UpdateAdministrador(Administrador administrador);
        Task<ResponseEntity<Administrador>> DeleteAdministrador(int id);
        Task<ResponseEntity<Administrador>> GetAllAdministradorRadicadores();

        Task<ResponseEntity<Administrador>> GetAllAdministradorDoctores();
        
        Task<ResponseEntity<Administrador>> GetBuscarAdminPanel(string adm_user, string adm_email, int? adm_nivel, int? adm_est);

        bool ChangePassword(LoginChangePasswordModel model);
    }
}
