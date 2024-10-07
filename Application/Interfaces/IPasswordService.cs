namespace Application.Interfaces
{
    public interface IPasswordService
    {
        public byte[] GenerateSalt();
        public string HashPassword(string password, byte[] salt);
        public bool VerifyPassword(string enteredPassword, string storedHash);


    }
}
