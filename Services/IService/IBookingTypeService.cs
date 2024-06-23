using BusinessObjects.Entities;

namespace Services.IService;

public interface IBookingTypeService
{
    List<BookingType> GetAllBookingTypes();
    void AddBookingType(BookingType bookingType);
    void DeleteBookingType(int bookingTypeId);
    BookingType GetBookingTypeById(int bookingTypeId);
    void UpdateBookingType(BookingType bookingType);
}