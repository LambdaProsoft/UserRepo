using Domain.Models;
using Application.Response;

namespace Application.Mappers.IMappers
{
    public interface IUserMapper
    {
        Task<UserResponse> GetUserResponse(User user);
    }

}
