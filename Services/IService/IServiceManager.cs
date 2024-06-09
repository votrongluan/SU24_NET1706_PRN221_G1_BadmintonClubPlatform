namespace Services.IService;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
}