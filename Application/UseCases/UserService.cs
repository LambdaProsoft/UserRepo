using Domain.Models;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappers.IMappers;
using Application.Request;
using Application.Response;

namespace Application.UseCases
{
    public class UserService : IUserService
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        private readonly IUserMapper _userMapper;

        private readonly IPasswordService _passwordService;

        private readonly IJwtService _jwtService;

        public UserService(IUserCommand userCommand, IUserQuery userQuery, IUserMapper userMapper, IPasswordService passwordService, IJwtService jwtService)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
            _userMapper = userMapper;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<UserResponse> CreateUser(UserRequest user)
        {

            await CheckUserRequest(user);

            var salt = _passwordService.GenerateSalt();
            var hashedPassword = _passwordService.HashPassword(user.Password, salt);

            var newUser = new User
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                DNI = user.DNI,
                Country = user.Country,
                City = user.City,
                Address = user.Adress,
                BirthDate = user.BirthDate,
                Phone = user.Phone,
                LastLogin = DateTime.UtcNow,
                Deleted = false,
                Password = hashedPassword

            };

            await _userCommand.CreateUser(newUser);
            var userRetrived = await _userQuery.GetUserById(newUser.Id);
            return await _userMapper.GetUserResponse(userRetrived);

        }
        public async Task<string> UserLogin(string email, string password)
        {
            var user = await _userQuery.GetUserEmail(email);
            if (user == null || !_passwordService.VerifyPassword(password, user.Password))
            {
                return null; // Credenciales incorrectas
            }

            // Generar el token JWT si las credenciales son correctas
            var token = _jwtService.GenerateJwtToken(user.Id, user.Email);

            return token;
        }
        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest command)
        {
            // Buscar al usuario por Email o Phone
            var user = await _userQuery.GetUserEmail(command.EmailOrPhone);
            if (user == null)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Verificar la contraseña actual
            if (!_passwordService.VerifyPassword(command.CurrentPassword, user.Password))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Current password is incorrect"
                };
            }

            // Generar nuevo salt
            var newSalt = _passwordService.GenerateSalt();

            // Hashear la nueva contraseña con el nuevo salt
            var newPasswordHash = _passwordService.HashPassword(command.NewPassword, newSalt);

            // Actualizar la contraseña en la base de datos
            user.Password = newPasswordHash;

            await _userCommand.UpdateUser(user);

            return new ChangePasswordResponse
            {
                Success = true,
                Message = "Password updated successfully"
            };
        }


        public async Task<UserResponse> DeleteUser(int userId)
        {
            await CheckUserId(userId);

            var softDelete = await _userCommand.DeleteUser(userId);
            return await _userMapper.GetUserResponse(softDelete);

        }

        public async Task<UserResponse> GetUserById(int id)
        {
            await CheckUserId(id);
            var user = await _userQuery.GetUserById(id);
            return await _userMapper.GetUserResponse(user);
        }


        public async Task<UserResponse> UpdateUser(int userId, UserUpdateRequest userRequest)
        {
            await CheckUserId(userId);
            var user = await _userQuery.GetUserById(userId);
            user.Email = userRequest.Email;
            user.Phone = userRequest.Phone;
            var result = await _userCommand.UpdateUser(user);
            return await _userMapper.GetUserResponse(result);
        }


        private async Task CheckUserRequest(UserRequest request)
        {

            if (await _userQuery.GetUserEmail(request.Email) != null)
            {
                throw new BadRequestException("A user with this email already exist");
            }

        }

        private async Task<bool> CheckUserId(int id)
        {
            if (await _userQuery.GetUserById(id) == null)
            {
                throw new ExceptionNotFound("There´s no user with that id");
            }
            return false;
        }
    }
}
