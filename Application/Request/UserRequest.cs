namespace Application.Request
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DNI { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }
        public int Phone { get; set; }

        //Modificacion para crear cuenta
        public int Currency { get; set; }
        public int AccountType { get; set; }
    }
}
