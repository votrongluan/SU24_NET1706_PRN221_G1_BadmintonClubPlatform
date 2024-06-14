using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class AccountService : IAccountService
{
    private readonly IRepositoryManager _repo;

    public AccountService(IRepositoryManager repo)
    {
        _repo = repo;
    }
}