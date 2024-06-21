using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Account
{
    public class AccountAddDto
    {
        [Required(ErrorMessage = "Cần nhập tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Cần chọn câu lạc bộ")]
        public int? ClubId { get; set; }
    }
}