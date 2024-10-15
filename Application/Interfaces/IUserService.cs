using Application.Request;
using Application.Response;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> DeleteUser(int userId);
        Task<UserResponse> CreateUser(UserRequest user);
        Task<string> UserLogin(string email, string password);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest command);
        public Task<TokenResponse> RefreshToken(string accessToken, string refreshToken);
        Task<UserResponse> UpdateUser(int userId, UserUpdateRequest userRequest);
        Task<UserResponse> GetUserById(int id);
    }
}
