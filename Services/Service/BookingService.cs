using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class BookingService : IBookingService
{
    private readonly IRepositoryManager _repo;

    public BookingService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<Booking> GetAllBookings()
    {
        return _repo.Booking.GetAllBookings();
    }

    public void AddBooking(Booking booking)
    {
        _repo.Booking.AddBooking(booking);
    }

    public void DeleteBooking(int bookingId)
    {
        _repo.Booking.DeleteBooking(bookingId);
    }

    public Booking GetBookingById(int bookingId)
    {
        return _repo.Booking.GetBookingById(bookingId);
    }

    public void UpdateBooking(Booking booking)
    {
        _repo.Booking.UpdateBooking(booking);
    }
}