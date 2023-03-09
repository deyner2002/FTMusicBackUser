using Core.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Services
{
    public interface IUserServices
    {
        Task<List<UserModel>> GetList();
        Task<long> SaveUser(UserModel user);
    }
}
