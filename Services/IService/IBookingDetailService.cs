using BusinessObjects.Entities;

namespace Services.IService;

public interface IBookingDetailService
{
    List<BookingDetail> GetAllBookingDetails();
    void AddBookingDetail(BookingDetail bookingDetail);
    void DeleteBookingDetail(int bookingDetailId);
    BookingDetail GetBookingDetailById(int bookingDetailId);
    void UpdateBookingDetail(BookingDetail bookingDetail);
    List<BookingDetail> GetBookingsByDateAndCourt(DateOnly date, int courtId);
}