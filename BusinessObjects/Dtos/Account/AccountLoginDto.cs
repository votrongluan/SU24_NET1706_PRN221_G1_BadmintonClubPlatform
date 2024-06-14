using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Account
{
    public class AccountLoginDto
    {
        [Required(ErrorMessage = "Cần phải nhập tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Cần phải nhập mật khẩu")]
        public string Password { get; set; }
    }
}
