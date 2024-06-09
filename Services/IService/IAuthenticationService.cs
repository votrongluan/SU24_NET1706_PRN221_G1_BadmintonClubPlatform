using BusinessObjects.Entities;

namespace Services.IService;

public interface IAuthenticationService
{
    Account Login(string username, string password);
    Account RegisterCustomerAccount(string username, string password);
    Account RegisterStaffAccount(string username, string password);
}