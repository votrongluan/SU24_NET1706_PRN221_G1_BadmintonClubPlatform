using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Club
{
    public class ClubDto
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Tên câu lạc bộ không được để trống")]
        public string ClubName { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Bạn phải chọn quận huyện")]
        public int DistrictId { get; set; }

        public string? FanpageLink { get; set; }

        public string? AvatarLink { get; set; }

        [Required(ErrorMessage = "Giờ mở cửa không được để trống")]
        public TimeOnly? OpenTime { get; set; }

        [Required(ErrorMessage = "Giờ đóng cửa không được để trống")]
        public TimeOnly? CloseTime { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        public string? ClubEmail { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string? ClubPhone { get; set; }

        [Required(ErrorMessage = "Client ID không được để trống")]
        public string? ClientId { get; set; }

        [Required(ErrorMessage = "API Key không được để trống")]
        public string? ApiKey { get; set; }

        [Required(ErrorMessage = "Checksum Key không được để trống")]
        public string? ChecksumKey { get; set; }

        public bool? Status { get; set; }

        public int? TotalStar { get; set; }

        public int? TotalReview { get; set; }

        [Required(ErrorMessage = "Giá mặc định không được để trống")]
        public int? DefaultPricePerHour { get; set; }
    }
}
