using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Dtos.Booking;

public class BookOfflineRequestDto
{
    [Required(ErrorMessage = "Phải chọn mã sân sân")]
    public int CourtId { get; set; }
    [Required(ErrorMessage = "Phải chọn giờ bắt đầu")]
    public TimeOnly StartTime { get; set; }
    [Required(ErrorMessage = "Phải chọn giờ kết thúc")]
    public TimeOnly EndTime { get; set; }
}