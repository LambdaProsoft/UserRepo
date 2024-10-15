namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DNI { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int Phone { get; set; }
        public bool Deleted { get; set; } = false;


        public ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}
