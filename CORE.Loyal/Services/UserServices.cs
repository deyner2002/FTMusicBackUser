using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Providers;
using CORE.Loyal.Interfaces.Services;
using Support.Loyal.Util;

namespace Core.Loyal.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserProvider _provider;
        public UserServices(IUserProvider provider)
        {
            _provider = provider;
        }
        public async Task<List<UserModel>> GetList()
        {
            List<UserModel> list = new List<UserModel>(); 
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
        public async Task<long> SaveUser(UserModel user)
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

    }
}
