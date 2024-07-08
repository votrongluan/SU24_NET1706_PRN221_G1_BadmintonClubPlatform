using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class BookingRepository : IBookingRepository
{
    public List<Booking> GetAllBookings()
    {
        return BookingDao.GetAll()
            .Include(b => b.User)
            .Include(b => b.Club)
            .ThenInclude(c => c.District)
                .ThenInclude(d => d.City)
            .Include(x => x.Club)
            .Include(x => x.BookingDetails)
            .ThenInclude(x => x.Court)
            .Include(b => b.BookingDetails)
            .Include(b => b.BookingType)
            .OrderByDescending(e => e.BookingId).ToList();
    }

    public List<Booking> GetAllBookingsWithBookingDetails()
    {
        return BookingDao.GetAll()
            .Include(b => b.BookingDetails)
            .Include(b => b.BookingType)
            .Include(b => b.User)
            .ToList();
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