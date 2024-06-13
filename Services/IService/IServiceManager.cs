namespace Services.IService;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IClubService ClubService { get; }
    ICityService CityService { get; }
    IDistrictService DistrictService { get; }
}