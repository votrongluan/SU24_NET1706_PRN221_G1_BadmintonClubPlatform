using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class BookingTypeService : IBookingTypeService
{
    private readonly IRepositoryManager _repo;

    public BookingTypeService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<BookingType> GetAllBookingTypes()
    {
        return _repo.BookingType.GetAllBookingTypes();
    }

    public void AddBookingType(BookingType bookingType)
    {
        _repo.BookingType.AddBookingType(bookingType);
    }

    public void DeleteBookingType(int bookingTypeId)
    {
        _repo.BookingType.DeleteBookingType(bookingTypeId);
    }

    public BookingType GetBookingTypeById(int bookingTypeId)
    {
        return _repo.BookingType.GetBookingTypeById(bookingTypeId);
    }

    public void UpdateBookingType(BookingType bookingType)
    {
        _repo.BookingType.UpdateBookingType(bookingType);
    }
}