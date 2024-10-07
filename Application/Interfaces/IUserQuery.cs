using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserQuery
    {
        Task<User> GetUserEmail(string email);

        Task<User> GetUserById(int id);
    }
}
