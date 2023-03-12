using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.FTMUSIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface IUsuarioProvider
    {
        Task<List<UsuarioModel>> GetList();
        Task<long> SaveUser(UsuarioModel user);
        Task<Boolean> ExistsUserCorreo(string correo);
        Task<Boolean> ValidarContrasenia(string correo,string contrasenia);
        Task<UsuarioModel> ConsultarUsuario(int Id);

        Task<long> ModificarUsuario(UsuarioModel user);

    }
}
