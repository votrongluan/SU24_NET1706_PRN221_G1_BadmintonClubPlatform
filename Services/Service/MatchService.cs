using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class MatchService : IMatchService
{
    private readonly IRepositoryManager _repo;

    public MatchService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}