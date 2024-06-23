using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IBookingTypeRepository
{
    List<BookingType> GetAllBookingTypes();
    BookingType GetBookingTypeById(int bookingTypeId);
    void DeleteBookingType(int bookingTypeId);
    void UpdateBookingType(BookingType bookingType);
    void AddBookingType(BookingType bookingType);
}