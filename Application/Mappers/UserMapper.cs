using Domain.Models;
using Application.Mappers.IMappers;
using Application.Response;

namespace Application.Mappers
{
    public class UserMapper : IUserMapper
    {
        public Task<UserResponse> GetUserResponse(User user)
        {
            var response = new UserResponse
            {
                Adress = user.Address,
                BirthDate = user.BirthDate,
                City = user.City,
                Country = user.Country,
                Deleted = user.Deleted,
                DNI = user.DNI,
                Email = user.Email,
                Id = user.Id,
                LastLogin = user.LastLogin,
                LastName = user.LastName,
                Name = user.Name,
                Phone = user.Phone,
            };
            return Task.FromResult(response);
        }
    }
}
