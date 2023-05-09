using CORE.Loyal.DBConnection;
using Core.Loyal.Models.FTMUSIC;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Loyal.Interfaces.Providers;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Providers
{
    public class UsuarioProvider :IUsuarioProvider
    {

        public ConnectionStrings _ConnectionStrings { get; set; }
        private OracleCommand cmd { get; set; }
        public UsuarioProvider(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
            cmd = new OracleCommand();
            cmd.Connection = OracleDBConnectionSingleton.OracleDBConnection.oracleConnection;
        }
        
        public async Task<List<UsuarioModel>> GetList()
        {
            var _outs = new List<UsuarioModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, NOMBRE,NOMBREARTISTICO, CORREO, FECHAREGISTRO,DESCRIPCION,FACEBOOK,INSTAGRAM,YOUTUBE,FECHANACIMIENTO FROM DBFTMUSIC.USUARIOS WHERE ESTADO= 'A'";
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        UsuarioModel usuarioModel = new UsuarioModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN NOMBRE",
                            NombreArtistico = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToString(item.ItemArray[2]) : "SIN NOMBRE ARTISTICO",
                            Correo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN CORREO",
                            FechaRegistro = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToDateTime(item.ItemArray[4]) : null,
                            Descripcion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]) : "SIN DESCRIPCION",
                            Facebook = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToString(item.ItemArray[6]) : "SIN FACEBOOK",
                            Instagram = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN INSTAGRAM",
                            Youtube = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN YOUTUBE",
                            FechaNacimiento = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[9]) ? Convert.ToDateTime(item.ItemArray[9]) : null
                        };


                        _outs.Add(usuarioModel);
                    }
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }
        
        public async Task<long> SaveUser(UsuarioModel user)
        {
            
            long consecutivo = 0;
            try
            {
                
                if (user.Nombre != null && user.Contrasenia != null && user.Correo != null )
                {



                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();


                    cmd.CommandText = @"
                                        select CORREO from DBFTMUSIC.USUARIOS WHERE CORREO=:P_CORREO AND ESTADO='A'
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = user.Correo });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count == 0)
                    {

                        cmd.CommandText = @"
                                        INSERT INTO DBFTMUSIC.USUARIOS
                                        (ID, NOMBRE,NOMBREARTISTICO, CORREO, CONTRASENIA,DESCRIPCION,FECHAREGISTRO,FECHANACIMIENTO,ESTADO,FACEBOOK,INSTAGRAM,YOUTUBE)
                                        VALUES(DBFTMUSIC.SEQUENCIAUSUARIO.NEXTVAL, :P_NOMBRE,:P_NOMBREARTISTICO, :P_CORREO, :P_CONTRASENIA, :P_DESCRIPCION, CURRENT_DATE,:P_FECHANACIMIENTO,'A',:P_FACEBOOK,:P_INSTAGRAM,:P_YOUTUBE)
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = user.Nombre });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBREARTISTICO", Value = user.NombreArtistico });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = user.Correo });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CONTRASENIA", Value = user.Contrasenia });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DESCRIPCION", Value = user.Descripcion });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Date, Direction = ParameterDirection.Input, ParameterName = "P_FECHANACIMIENTO", Value = user.FechaNacimiento });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_FACEBOOK", Value = user.Facebook });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_INSTAGRAM", Value = user.Instagram });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_YOUTUBE", Value = user.Youtube });


                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = @"
                                        select DBFTMUSIC.SEQUENCIAUSUARIO.currval from dual
                                        ";
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                        if (data.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in data.Tables[0].Rows)
                            {
                                consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                            }
                        }
                        return consecutivo;

                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }
        
        public async Task<UsuarioModel> ConsultarUsuario(string correo,string contrasenia)
        {
            var _outs = new List<UsuarioModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, NOMBRE, NOMBREARTISTICO, CORREO, FECHAREGISTRO,DESCRIPCION,FACEBOOK,INSTAGRAM,YOUTUBE,FECHANACIMIENTO,CONTRASENIA FROM DBFTMUSIC.USUARIOS WHERE CORREO=:P_CORRREO AND CONTRASENIA=:P_CONTRASENIA AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = correo });
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CONTRASENIA", Value = contrasenia });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        UsuarioModel usuarioModel = new UsuarioModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN NOMBRE",
                            NombreArtistico = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToString(item.ItemArray[2]) : "SIN NOMBRE ARTISTICO",
                            Correo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN CORREO",
                            FechaRegistro = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToDateTime(item.ItemArray[4]) : null,
                            Descripcion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]) : "SIN DESCRIPCION",
                            Facebook = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToString(item.ItemArray[6]) : "SIN FACEBOOK",
                            Instagram = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN INSTAGRAM",
                            Youtube = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN YOUTUBE",
                            FechaNacimiento = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[9]) ? Convert.ToDateTime(item.ItemArray[9]) : null,
                            Contrasenia = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[10]) ? Convert.ToString(item.ItemArray[10]) : "SIN CONTRASENIA"

                        };
                        _outs.Add(usuarioModel);
                    }
                }
                return _outs[0];
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }





        public async Task<UsuarioModel> ConsultarUsuario(int Id)
        {
            var _outs = new List<UsuarioModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, NOMBRE,NOMBREARTISTICO, CORREO, FECHAREGISTRO,DESCRIPCION,FACEBOOK,INSTAGRAM,YOUTUBE,FECHANACIMIENTO,CONTRASENIA FROM DBFTMUSIC.USUARIOS WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = Id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        UsuarioModel usuarioModel = new UsuarioModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN NOMBRE",
                            NombreArtistico = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToString(item.ItemArray[2]) : "SIN NOMBRE ARTISTICO",
                            Correo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN CORREO",
                            FechaRegistro = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToDateTime(item.ItemArray[4]) : null,
                            Descripcion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]) : "SIN DESCRIPCION",
                            Facebook = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToString(item.ItemArray[6]) : "SIN FACEBOOK",
                            Instagram = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN INSTAGRAM",
                            Youtube = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN YOUTUBE",
                            FechaNacimiento = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[9]) ? Convert.ToDateTime(item.ItemArray[9]) : null,
                            Contrasenia = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[10]) ? Convert.ToString(item.ItemArray[10]) : "SIN CONTRASENIA"
                        };


                        _outs.Add(usuarioModel);
                    }
                }

                return _outs[0];
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }


        public async Task<long> ModificarUsuario(UsuarioModel user)
        {

            long consecutivo = 0;
            try
            {
                
                if (user.Nombre != null && user.Contrasenia != null && user.Correo != null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = @"
                                        select CORREO from DBFTMUSIC.USUARIOS WHERE CORREO=:P_CORREO AND ESTADO='A'
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = user.Correo });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);



                    if (data.Tables[0].Rows.Count == 0)
                    {

                        cmd.CommandText = @"
                                        UPDATE DBFTMUSIC.USUARIOS SET NOMBRE=:P_NOMBRE,
                                        NOMBREARTISTICO=:P_NOMBREARTISTICO,
                                        CORREO=:P_CORREO, 
                                        CONTRASENIA=:P_CONTRASENIA,
                                        DESCRIPCION=:P_DESCRIPCION,
                                        FECHANACIMIENTO=:P_FECHANACIMIENTO,
                                        FACEBOOK=:P_FACEBOOK,
                                        INSTAGRAM=:P_INSTAGRAM,
                                        YOUTUBE=:P_YOUTUBE
                                        WHERE ID = :P_ID AND ESTADO ='A'
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = user.Nombre });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBREARTISTICO", Value = user.NombreArtistico });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = user.Correo });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CONTRASENIA", Value = user.Contrasenia });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DESCRIPCION", Value = user.Descripcion });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Date, Direction = ParameterDirection.Input, ParameterName = "P_FECHANACIMIENTO", Value = user.FechaNacimiento });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_FACEBOOK", Value = user.Facebook });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_INSTAGRAM", Value = user.Instagram });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_YOUTUBE", Value = user.Youtube });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = user.Id });
                        await cmd.ExecuteNonQueryAsync();
                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    }
                    else
                    {
                        return 0;
                    }



                    
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();


                    return 1;
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }





        public async Task<long> DesactivarUsuario(int id)
        {

            long consecutivo = 0;
            try
            {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                        cmd.CommandText = @"
                                        UPDATE DBFTMUSIC.USUARIOS SET
                                        ESTADO='I'
                                        WHERE ID=:P_ID
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                        await cmd.ExecuteNonQueryAsync();
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    return 1;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }




        public async Task<long> GuardarSuscripcion(SuscripcionModel suscripcionModel)
        {   long consecutivo = 0;
            try
            {
                if (suscripcionModel.IdCantante!=null && suscripcionModel.IdSeguidor!=null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = @"
                                        SELECT ID,ID_CANTANTE,ID_SEGUIDOR FROM DBFTMUSIC.SUSCRIPCIONES WHERE ID_CANTANTE=:P_ID_CANTANTEP AND ID_SEGUIDOR=:P_ID_SEGUIDORP
                                   ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID_CANTANTEP", Value = suscripcionModel.IdCantante });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID_SEGUIDORP", Value = suscripcionModel.IdSeguidor });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);
                    if (data.Tables[0].Rows.Count == 0)
                    {
                        cmd.CommandText = @"
                                        INSERT INTO DBFTMUSIC.SUSCRIPCIONES(ID,ID_CANTANTE,ID_SEGUIDOR)
                                        VALUES(DBFTMUSIC.SEQUENCESUSCRIPCIONES.NEXTVAL,:P_ID_CANTANTE ,:P_ID_SEGUIDOR )
                                           ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID_CANTANTE", Value = suscripcionModel.IdCantante });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID_SEGUIDOR", Value = suscripcionModel.IdSeguidor });
                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = @"
                                        select DBFTMUSIC.SEQUENCESUSCRIPCIONES.currval from dual
                                        ";
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        

                        if (data.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in data.Tables[0].Rows)
                            {
                                consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                            }
                        }

                    }
                    else
                    {
                        consecutivo = -2;   //ya se encuentra suscrito
                    }
                }
                else
                {
                    consecutivo = -3;    //campos vacios
                }
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                consecutivo = -1;
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return consecutivo;
        }


    }
}
