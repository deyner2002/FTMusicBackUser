using CORE.Loyal.Interfaces.Providers;
using Core.Loyal.Models.FTMUSIC;
using Support.Loyal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Loyal.Interfaces.Services;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Services
{
    public class UsuarioServices: IUsuarioServices
    {
        private readonly IUsuarioProvider _provider;
        public UsuarioServices(IUsuarioProvider provider)
        {
            _provider = provider;
        }
        public async Task<List<UsuarioModel>> GetList()
        {
            List<UsuarioModel> list = new List<UsuarioModel>();
            try
            {
                list = await _provider.GetList();
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }

        public async Task<long> SaveUser(UsuarioModel user)
        {
            long consecutivo = 0;
            try
            {
                consecutivo = await _provider.SaveUser(user);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return consecutivo;
        }


        public async Task<UsuarioModel> ConsultarUsuario(string correo, string contrasenia)
        {
            UsuarioModel usuario = new UsuarioModel();
            try
            {
                usuario = await _provider.ConsultarUsuario(correo,contrasenia);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return usuario;
        }

        public async Task<UsuarioModel> ConsultarUsuario(int Id)
        {
            UsuarioModel usuario = new UsuarioModel();
            try
            {
                usuario = await _provider.ConsultarUsuario(Id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return usuario;
        }


        public async Task<long> ModificarUsuario(UsuarioModel user)
        {
            long result = 0;
            try
            {
                result = await _provider.ModificarUsuario(user);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return result;
        }

        public async Task<long> DesactivarUsuario(int id)
        {
            long result = -1;
            try
            {
                result = await _provider.DesactivarUsuario(id);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return result;
        }

    }
}
