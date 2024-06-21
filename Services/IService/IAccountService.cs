using BusinessObjects.Entities;

namespace Services.IService;

public interface IAccountService
{
    Account GetAccount (string username, string password);
    void RegisterAccount (Account account);
    bool CheckUsernameExisted (string username);
    bool CheckPhoneExisted (string phone);
    bool CheckEmailExisted (string email);
    List<Account> GetAllAccount ();
    List<Account> GetAllStaffAccount ();
}