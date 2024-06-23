using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IBookingDetailRepository
{
    List<BookingDetail> GetAllBookingDetails();
    BookingDetail GetBookingDetailById(int bookingDetailId);
    void DeleteBookingDetail(int bookingDetailId);
    void UpdateBookingDetail(BookingDetail bookingDetail);
    void AddBookingDetail(BookingDetail bookingDetail);
}