using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class BookingTypeRepository : IBookingTypeRepository
{
    public List<BookingType> GetAllBookingTypes()
    {
        return BookingTypeDao.GetAll().ToList();
    }

    public BookingType GetBookingTypeById(int bookingTypeId)
    {
        return GetAllBookingTypes().FirstOrDefault(e => e.BookingTypeId == bookingTypeId);
    }

    public void DeleteBookingType(int bookingTypeId)
    {
        var bookingType = GetBookingTypeById(bookingTypeId);
        BookingTypeDao.Delete(bookingType);
    }

    public void UpdateBookingType(BookingType bookingType)
    {
        BookingTypeDao.Update(bookingType);
    }

    public void AddBookingType(BookingType bookingType)
    {
        BookingTypeDao.Add(bookingType);
    }
}