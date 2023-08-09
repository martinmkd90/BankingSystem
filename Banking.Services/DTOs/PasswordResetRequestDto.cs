using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.DTOs
{
    public class PasswordResetRequestDto
    {
        public string Email { get; set; } = string.Empty;
    }

}
