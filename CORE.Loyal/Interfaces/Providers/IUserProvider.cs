using Core.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface IUserProvider
    {
        Task<List<UserModel>> GetList();
        Task<long> SaveUser(UserModel user);
    }
}
