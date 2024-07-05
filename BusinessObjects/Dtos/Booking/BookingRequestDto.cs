using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Dtos.Booking;

public class BookingRequestDto
{
    public int DefaultPrice { get; set; }
    public TimeOnly StartTime;
    public TimeOnly EndTime;
    public int ClubId { get; set; }
    public int UserId { get; set; }
    [Required(ErrorMessage = "Phải chọn loại sân")]
    public int CourtTypeId { get; set; }
    [Required(ErrorMessage = "Phải chọn ngày đặt")]
    public DateOnly BookDate { get; set; }
    [Required(ErrorMessage = "Phải chọn loại dịch vụ")]
    public int BookingTypeId { get; set; }

    [Required(ErrorMessage = "Phải nhập giờ bắt đầu")]
    [AllowedValues(7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, ErrorMessage = "Chỉ nhập giờ trong khoảng 7 - 20")]
    public int StartTimeHour { get; set; }

    [Required(ErrorMessage = "Phải chọn số phút")]
    public int StartTimeMinute { get; set; }
    [Required(ErrorMessage = "Phải chọn thời lượng")]
    [AllowedValues(1, 2, 3, ErrorMessage = "Chỉ chọn từ 1 đến 3 tiếng")]
    public int Duration { get; set; }
    public int WeekCount { get; set; }
}