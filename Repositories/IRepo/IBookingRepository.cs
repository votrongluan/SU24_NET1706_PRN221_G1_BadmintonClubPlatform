using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IBookingRepository
{
    List<Booking> GetAllBookings();
    List<Booking> GetAllBookingsWithBookingDetails();
    Booking GetBookingById(int bookingId);
    void DeleteBooking(int bookingId);
    void UpdateBooking(Booking booking);
    void AddBooking(Booking booking);
}