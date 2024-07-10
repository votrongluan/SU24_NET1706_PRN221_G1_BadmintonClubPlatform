using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class BookingDetailService : IBookingDetailService
{
    private readonly IRepositoryManager _repo;

    public BookingDetailService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<BookingDetail> GetAllBookingDetails()
    {
        return _repo.BookingDetail.GetAllBookingDetails();
    }

    public void AddBookingDetail(BookingDetail bookingDetail)
    {
        _repo.BookingDetail.AddBookingDetail(bookingDetail);
    }

    public void DeleteBookingDetail(int bookingDetailId)
    {
        _repo.BookingDetail.DeleteBookingDetail(bookingDetailId);
    }

    public BookingDetail GetBookingDetailById(int bookingDetailId)
    {
        return _repo.BookingDetail.GetBookingDetailById(bookingDetailId);
    }

    public void UpdateBookingDetail(BookingDetail bookingDetail)
    {
        _repo.BookingDetail.UpdateBookingDetail(bookingDetail);
    }

    public List<BookingDetail> GetBookingsByDateAndCourt(DateOnly date, int courtId)
    {
        return _repo.BookingDetail.GetAllBookingDetails()
            .Where(bd => bd.BookDate == date && bd.CourtId == courtId).OrderBy(e => e.StartTime)
            .ToList();
    }
}