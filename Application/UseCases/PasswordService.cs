using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;


namespace Application.UseCases
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;

        //Genera un salt único para cada usuario.

        public byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Realiza el hashing de la contraseña junto con el salt generado.
        public string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenar la contraseña con el salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hashing de la contraseña + salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenar el salt y el hash para almacenar
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                // Convertir el resultado a Base64 para almacenamiento
                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }

        // Verifica una contraseña dada comparándola con el hash almacenado.
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Convertir el hash almacenado de Base64 a bytes
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            // Extraer el salt (los primeros 16 bytes)
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(storedHashBytes, 0, salt, 0, SaltSize);

            // Hash de la contraseña introducida con el salt extraído
            string enteredPasswordHash = HashPassword(enteredPassword, salt);

            // Comparar el hash generado con el hash almacenado
            return storedHash == enteredPasswordHash;
        }
    }
}
