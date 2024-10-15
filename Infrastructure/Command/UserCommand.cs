using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using UserInfrastructure.Persistence;

namespace Infrastructure.Command
{
    public class UserCommand : IUserCommand
    {
        private readonly UserContext _context;
        public UserCommand(UserContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException ex)
            {
                // Capturar la excepción interna para obtener más detalles
                var innerException = ex.InnerException?.Message;
                throw new Exception($"Error occurred while saving the entity changes: {innerException}");
            }

        }

        public async Task<User> DeleteUser(int UserId)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            userToDelete.Deleted = true;
            await _context.SaveChangesAsync();
            return userToDelete;
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateRefreshToken(int userId, string newRefreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .FirstOrDefaultAsync();

            if (tokenEntity != null)
            {
                // Actualizar el Refresh Token existente
                tokenEntity.Token = newRefreshToken;
                tokenEntity.ExpirationDate = DateTime.UtcNow.AddDays(30); // Actualizar la fecha de expiración

                _context.RefreshTokens.Update(tokenEntity);
            }
            else
            {
                // Crear un nuevo Refresh Token si no existe
                tokenEntity = new RefreshToken
                {
                    UserId = userId,
                    Token = newRefreshToken,
                    ExpirationDate = DateTime.UtcNow.AddDays(30)
                };

                await _context.RefreshTokens.AddAsync(tokenEntity);
            }

            await _context.SaveChangesAsync();
        }

        //public async Task UpdateRefreshToken(int userId, string newRefreshToken)
        //{
        //    var tokenEntity = await _context.RefreshTokens
        //        .Where(t => t.UserId == userId)
        //        .FirstOrDefaultAsync();

        //    if (tokenEntity != null)
        //    {
        //        tokenEntity.Token = newRefreshToken;
        //        tokenEntity.ExpirationDate = DateTime.UtcNow.AddDays(30); // Actualizar la fecha de expiración

        //        _context.RefreshTokens.Update(tokenEntity);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        throw new Exception("No se encontró el Refresh Token para este usuario.");
        //    }
        //}
    }
}
