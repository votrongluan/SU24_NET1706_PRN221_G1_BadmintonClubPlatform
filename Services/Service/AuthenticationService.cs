using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repoManager;

    public AuthenticationService(IRepositoryManager repoManager)
    {
        _repoManager = repoManager;
    }

    public Account Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Account RegisterCustomerAccount(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Account RegisterStaffAccount(string username, string password)
    {
        throw new NotImplementedException();
    }
}