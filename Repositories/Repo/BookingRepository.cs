using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class BookingRepository : IBookingRepository
{
    public List<Booking> GetAllBookings()
    {
        return BookingDao.GetAll().ToList();
    }

    public Booking GetBookingById(int bookingId)
    {
        return GetAllBookings().FirstOrDefault(e => e.BookingId == bookingId);
    }

    public void DeleteBooking(int bookingId)
    {
        var booking = GetBookingById(bookingId);
        BookingDao.Delete(booking);
    }

    public void UpdateBooking(Booking booking)
    {
        BookingDao.Update(booking);
    }

    public void AddBooking(Booking booking)
    {
        BookingDao.Add(booking);
    }
}