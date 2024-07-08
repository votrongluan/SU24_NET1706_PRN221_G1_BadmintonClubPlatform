using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;

namespace Services.IService;

public interface IBookingService
{
    List<Booking> GetAllBookings();
    List<Booking> GetAllBookingsWithBookingDetails();
    void AddBooking(Booking booking);
    void DeleteBooking(int bookingId);
    Booking GetBookingById(int bookingId);
    void UpdateBooking(Booking booking);
    (bool status, int bookId) BookLichThiDau(BookingRequestDto dto);
    bool BookLichNgay(BookingRequestDto dto);
    bool BookLichCoDinh(BookingRequestDto dto);
}