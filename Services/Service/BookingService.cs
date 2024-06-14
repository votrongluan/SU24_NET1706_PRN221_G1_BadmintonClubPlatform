using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class BookingService : IBookingService
{
    private readonly IRepositoryManager _repo;

    public BookingService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}