using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class ChangePasswordRequest
    {
        public string EmailOrPhone { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordRequest(string emailOrPhone, string currentPassword, string newPassword)
        {
            EmailOrPhone = emailOrPhone;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}
