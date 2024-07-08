using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IBookingRepository
{
    Booking GetBookingByIdNoInclude(int bookingId);
    List<Booking> GetAllBookings();
    List<Booking> GetAllBookingsWithBookingDetails();
    Booking GetBookingById(int bookingId);
    void DeleteBooking(int bookingId);
    void UpdateBooking(Booking booking);
    void AddBooking(Booking booking);
}