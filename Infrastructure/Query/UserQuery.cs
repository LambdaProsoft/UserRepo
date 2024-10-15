using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using Application.Interfaces;
using UserInfrastructure.Persistence;

namespace UserInfrastructure.Query
{
    public class UserQuery : IUserQuery
    {
        private readonly UserContext _context;

        public UserQuery(UserContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int userID)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(p => p.Id == userID);

                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Conflict("There was an issue getting the user by id in the database.", ex);
            }
        }

        public async Task<User> GetUserEmail(string email)
        {
            try
            {
                var existingEmail = await _context.Users
                .Where(p => p.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

                return existingEmail;
            }
            catch (DbUpdateException ex)
            {
                throw new Conflict("There was an issue getting the user email in the database.", ex);
            }

        }

        public async Task<RefreshToken> GetRefreshToken(int userId, string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Where(t => t.UserId == userId && t.Token == refreshToken)
                .FirstOrDefaultAsync();

            if (tokenEntity == null)
            {
                throw new Exception("Refresh Token no encontrado o inválido.");
            }

            return tokenEntity;
        }
    }
}
