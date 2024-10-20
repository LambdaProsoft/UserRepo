using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class VerificationCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; } = false;
        public User User { get; set; }
    }
}
