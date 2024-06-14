using BusinessObjects.Entities;

namespace Services.IService;

public interface IAccountService
{
    Account GetAccount (string username, string password);
}