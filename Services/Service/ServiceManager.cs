using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager _repoManager;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IRepositoryManager repoManager)
    {
        _repoManager = repoManager;
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_repoManager));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}