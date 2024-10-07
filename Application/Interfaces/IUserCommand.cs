using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserCommand
    {
        Task<User> CreateUser(User user);
        Task<User> DeleteUser(int UserId);
        Task<User> UpdateUser(User user);
    }
}
