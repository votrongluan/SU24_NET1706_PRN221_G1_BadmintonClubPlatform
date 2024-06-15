using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Account
{
    public class AccountResponseDto
    {
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
    }
}
