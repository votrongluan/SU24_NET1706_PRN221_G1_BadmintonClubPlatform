using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Entities
{
    public partial class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Tên câu lạc bộ là bắt buộc.")]
        public string ClubName { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Quận / huyện là bắt buộc.")]
        public int DistrictId { get; set; }

        [Url(ErrorMessage = "Liên kết fanpage không hợp lệ.")]
        public string? FanpageLink { get; set; }

        [Url(ErrorMessage = "Liên kết avatar không hợp lệ.")]
        public string? AvatarLink { get; set; }

        public TimeOnly? OpenTime { get; set; }

        public TimeOnly? CloseTime { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? ClubEmail { get; set; }

        [RegularExpression(@"^(0|\+84)?[1-9][0-9]{8}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? ClubPhone { get; set; }

        public string? ClientId { get; set; }

        public string? ApiKey { get; set; }

        public string? ChecksumKey { get; set; }

        public bool? Status { get; set; }

        public int? TotalStar { get; set; }

        public int? TotalReview { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

        public virtual ICollection<AvailableBookingType> AvailableBookingTypes { get; set; } = new List<AvailableBookingType>();

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<Court> Courts { get; set; } = new List<Court>();

        public virtual District District { get; set; } = null!;

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
