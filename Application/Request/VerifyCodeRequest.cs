using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class VerifyCodeRequest
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
