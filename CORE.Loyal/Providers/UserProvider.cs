using CORE.Loyal.DBConnection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System.Data;
using CORE.Loyal.Interfaces.Providers;
using Core.Loyal.Models.FTMUSIC;

namespace Core.Loyal.Providers
{
    public class UserProvider : IUserProvider
    {
        public ConnectionStrings _ConnectionStrings { get; set; }
        private OracleCommand cmd { get; set; }
        public UserProvider(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
            cmd = new OracleCommand();
            cmd.Connection = OracleDBConnectionSingleton.OracleDBConnection.oracleConnection;
        }
        public async Task<List<UserModel>> GetList()
        {
            var _outs = new List<UserModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                
                cmd.CommandText = "SELECT CONSECUTIVO, NOMBRE, CORREO, CLAVE FROM DBFTMUSIC.TUSER";
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        UserModel userModel = new UserModel
                        {
                            CONSECUTIVO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            NOMBRE = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN NOMBRE",
                            CORREO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToString(item.ItemArray[2]) : "SIN CORREO",
                            CLAVE = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN CLAVE"
                        };
                        _outs.Add(userModel);
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

        public async Task<long> SaveUser(UserModel user)
        {
            long consecutivo = 0;
            try
            {
                if (user.NOMBRE != null && user.CLAVE != null && user.CORREO != null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        INSERT INTO DBFTMUSIC.TUSER
                                        (CONSECUTIVO, NOMBRE, CORREO, CLAVE)
                                        VALUES(DBFTMUSIC.SEQUENCEUSER.NEXTVAL, :P_NOMBRE, :P_CORREO, :P_CLAVE)
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = user.NOMBRE });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CORREO", Value = user.CORREO });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_CLAVE", Value = user.CLAVE });

                    await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = @"
                                        select DBFTMUSIC.SEQUENCEUSER.currval from dual
                                        ";
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
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
                    return -2;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }
    }
}
