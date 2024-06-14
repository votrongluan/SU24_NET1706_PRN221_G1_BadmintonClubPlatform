using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class SlotService : ISlotService
{
    private readonly IRepositoryManager _repo;

    public SlotService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}