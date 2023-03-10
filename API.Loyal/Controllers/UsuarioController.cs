using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Services;
using CORE.Loyal.Models.FTMUSIC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System.Timers;

namespace API.Loyal.Controllers
{

    
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController:ControllerBase
    {


        private readonly IUsuarioServices _provider;

        public UsuarioController(IUsuarioServices provider)
        {
            _provider = provider;
        }


        
        [HttpGet("ConsultarUsuarios")]
        public async Task<ResponseModels> GetList()
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GetList().Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }
        
        
        
        [HttpPost("Registrar usuario")]
        public async Task<ResponseModels> Save(UsuarioModel user)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.SaveUser(user).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        response.IsError = false;
                        response.Mensaje = "Registro Guardado";
                    }
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }

        
        
        [HttpPost("ValidarExistenciaCorreo")]
        public async Task<ResponseModels> ExistsUsuarioCorreo(string correo)
        {
            Boolean exists = true;
            ResponseModels response = new ResponseModels();
            try
            {
                response.Datos=_provider.ExistsUserCorreo(correo).Result;
                exists= Boolean.Parse(response.Datos.ToString());
                response.Mensaje = "El usuario no existe";
                if (exists==true)
                {
                    response.Mensaje = "El usuario ya existe";
                }
               

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "No se pudo validar el usuario";
            }
            return response;
            
        }

        
        [HttpPost("ConsultarUsuarioPorCorreoYContrasenia")]
        public async Task<ResponseModels> ConsultarUsuario(string correo, string contrasenia)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                response.Datos = _provider.ConsultarUsuario(correo, contrasenia).Result;
                response.Mensaje = "Login Correcto";
                if (response.Datos == null)
                {
                    response.Mensaje = "Contrasenia o usuario incorrectos";
                }


            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "No se pudo validar el usuario";
            }
            return response;
        }



        
        [HttpGet("ConsultarUsuarioPorId")]
        public async Task<ResponseModels> ConsultarUsuario(int Id)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarUsuario(Id).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }




        
        [HttpPost("Modificar usuario")]
        public async Task<ResponseModels> ModificarUsuario(UsuarioModel user)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ModificarUsuario(user).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    
                    if(codigoRespuesta == 1)
                    {
                        response.IsError = false;
                        response.Mensaje = "Registro Modificado";
                    }

                    if (codigoRespuesta == -3)
                    {
                        response.IsError = true;
                        response.Mensaje = "El correo a modificar ya se encuentra en uso";
                    }

                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }





    }
}
