using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserCommand
    {
        Task<User> CreateUser(User user);
        Task UpdateRefreshToken(int userId, string newRefreshToken);
        Task<User> DeleteUser(int UserId);
        Task<User> UpdateUser(User user);
        Task SaveVerificationCode(int userId, string verificationCode);

        Task MarkCodeAsUsed(VerificationCode verificationCode);
    }
}
