using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class BookingDetailRepository : IBookingDetailRepository
{
    public List<BookingDetail> GetAllBookingDetails()
    {
        return BookingDetailDao.GetAll().ToList();
    }

    public BookingDetail GetBookingDetailById(int bookingDetailId)
    {
        return GetAllBookingDetails().FirstOrDefault(e => e.BookingDetailId == bookingDetailId);
    }

    public void DeleteBookingDetail(int bookingDetailId)
    {
        var bookingDetail = GetBookingDetailById(bookingDetailId);
        BookingDetailDao.Delete(bookingDetail);
    }

    public void UpdateBookingDetail(BookingDetail bookingDetail)
    {
        BookingDetailDao.Update(bookingDetail);
    }

    public void AddBookingDetail(BookingDetail bookingDetail)
    {
        BookingDetailDao.Add(bookingDetail);
    }
}