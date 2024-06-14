using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ReviewService : IReviewService
{
    private readonly IRepositoryManager _repo;

    public ReviewService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}