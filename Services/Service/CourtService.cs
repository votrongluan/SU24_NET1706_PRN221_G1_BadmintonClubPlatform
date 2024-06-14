using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class CourtService : ICourtService
{
    private readonly IRepositoryManager _repo;

    public CourtService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}