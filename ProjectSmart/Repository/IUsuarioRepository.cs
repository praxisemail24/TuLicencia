using SmartLicencia.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLicencia.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllUsuarios();
        Task<Usuario> GetUsuarioById(int id);
        Task AddUsuario(Usuario usuario); 
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(int id);

        public IEnumerable<string> ListarCorreosUsuario(string query);
    }
}
