using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserQuery
    {
        Task<User> GetUserEmail(string email);
        Task<RefreshToken> GetRefreshToken(int userId, string refreshToken);
        Task<User> GetUserById(int id);
    }
}
