using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Dtos.Booking;

public class BookingRequestDto
{
    [Required(ErrorMessage = "Phải chọn loại sân")]
    public int CourtTypeId { get; set; }
    [Required(ErrorMessage = "Phải chọn ngày đặt")]
    public DateOnly BookDate { get; set; }
    [Required(ErrorMessage = "Phải chọn loại dịch vụ")]
    public int BookingTypeId { get; set; }
    [Required(ErrorMessage = "Phải chọn khung giờ")]
    public int SlotId { get; set; }
    public int WeekCount { get; set; }
}