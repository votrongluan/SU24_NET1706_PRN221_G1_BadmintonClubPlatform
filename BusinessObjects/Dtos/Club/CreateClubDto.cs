using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Dtos.Club;

public class CreateClubDto
{
    [Required(ErrorMessage = "Cần phải nhập tên cho câu lạc bộ")]
    [MinLength(4, ErrorMessage = "Tối thiểu 4 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string ClubName { get; set; } = null!;

    [Required(ErrorMessage = "Cần phải chọn quận huyện")]
    public int DistrictId { get; set; }

    [Required(ErrorMessage = "Cần phải nhập địa chỉ")]
    [MinLength(4, ErrorMessage = "Tối thiểu 4 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string Address { get; set; } = null!;

    [RegularExpression(@"^(03|05|07|08|09|01[2689])+([0-9]{8})\b", ErrorMessage = "Số điện thoại phải đúng định dạng")]
    public string? ClubPhone { get; set; }
}