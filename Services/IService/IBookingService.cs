using BusinessObjects.Entities;

namespace Services.IService;

public interface IBookingService
{
    List<Booking> GetAllBookings();
    void AddBooking(Booking booking);
    void DeleteBooking(int bookingId);
    Booking GetBookingById(int bookingId);
    void UpdateBooking(Booking booking);
}