using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager _repoManager;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IClubService> _clubService;
    private readonly Lazy<ICityService> _cityService;
    private readonly Lazy<IDistrictService> _districtService;

    public ServiceManager(IRepositoryManager repoManager)
    {
        _repoManager = repoManager;
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_repoManager));
        _clubService = new Lazy<IClubService>(() => new ClubService(_repoManager));
        _cityService = new Lazy<ICityService>(() => new CityService(repoManager));
        _districtService = new Lazy<IDistrictService>(() => new DistrictService(repoManager));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IClubService ClubService => _clubService.Value;
    public ICityService CityService => _cityService.Value;
    public IDistrictService DistrictService => _districtService.Value;
}