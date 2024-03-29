﻿using Core.Loyal.Models.FTMUSIC;
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
        
        
        
        [HttpPost("RegistrarUsuario")]
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
                        if (codigoRespuesta==0)
                        {
                            response.IsError = true;
                            response.Mensaje = "El correo ya se encuentra en uso";
                        }
                        else {
                            response.IsError = false;
                            response.Mensaje = "Registro Guardado";

                        }
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
                    response.IsError = true;
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




        
        [HttpPost("ModificarUsuario")]
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
                    if (codigoRespuesta == 0)
                    {
                        response.IsError = true;
                        response.Mensaje = "El Correo que desea modificar ya se encuentra en uso";
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


        [HttpPost("DesactivarUsuario")]
        public async Task<ResponseModels> DesactivarUsuario(int id)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.DesactivarUsuario(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    response.IsError = false;
                    response.Mensaje = "Usuario Desactivado";
                }
                else
                {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
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


        [HttpPost("GuardarSuscripcion")]
        public async Task<ResponseModels> GuardarSuscripcion(SuscripcionModel suscripcionModel)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarSuscripcion(suscripcionModel).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -3)
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
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "El usuario ya se encuentra suscrito a este cantante";
                        }
                        else
                        {
                            response.IsError = false;
                            response.Mensaje = "suscripcion Guardada";

                        }
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


        [HttpGet("ConsultarSuscripcionesUsuario")]
        public async Task<ResponseModels> ConsultarSuscripcionesUsuario(int idSeguidor)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarSuscripcionesUsuario(idSeguidor).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "No se pudieron obtener datos";
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

        [HttpPost("EliminarSuscripcion")]
        public async Task<ResponseModels> EliminarSuscripcion(SuscripcionModel suscripcionModel)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.EliminarSuscripcion(suscripcionModel).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -3)
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
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "El usuario no se encuentra suscrito a este cantante";
                        }
                        else
                        {
                            response.IsError = false;
                            response.Mensaje = "suscripcion Eliminada";

                        }
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




        [HttpPost]
        [Route("ConsultarNumeroSeguidoresPorUsuario")]
        public async Task<ResponseModels> ConsultarNumeroSeguidoresPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.ConsultarNumeroSeguidoresPorUsuario(idUsuario).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta == -2)
                    {
                        intentosRestantes = 0;
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }

                    if (codigoRespuesta == -1)
                    {
                        intentosRestantes = 0;
                        response.IsError = true;
                        response.Mensaje = "El cantante en cuestion no tiene Seguidores";
                    }

                    if (codigoRespuesta >= 0)
                    {
                        intentosRestantes = 0;
                        response.IsError = false;
                        response.Mensaje = "El numero de Seguidores es de: " + codigoRespuesta;
                    }

                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }
            }
            return response;
        }



        [HttpPost]
        [Route("ValidarSuscripcionSeguidor")]
        public async Task<ResponseModels> ValidarSuscripcionSeguidor(int idCantante, int idSeguidor)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.ValidarSuscripcionSeguidor( idCantante,  idSeguidor).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta == 1)
                    {
                        response.IsError = false;
                        response.Mensaje = "El usuario no se encuentra suscrito a este cantante";
                        intentosRestantes = 0;
                    }
                    else
                    {

                        if (codigoRespuesta == 2)
                        {
                            response.IsError = false;
                            response.Mensaje = "El usuario se encuentra suscrito a este cantante";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "campos obligatorios se encuentran vacios";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -1)
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            await Task.Delay(1000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }


    }
}
