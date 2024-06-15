using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Account
{
    public class AccountRegisterDto
    {
        [Required(ErrorMessage = "Cần nhập tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Cần nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"^(03|05|07|08|09|01[2689])+([0-9]{8})\b", ErrorMessage = "Số điện thoại phải đúng định dạng")]
        [Required(ErrorMessage = "Cần nhập số điện thoại")]
        public string UserPhone { get; set; }

        public string? Email { get; set; }
    }
}
