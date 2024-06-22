using BusinessObjects.Entities;

namespace Services.IService;

public interface IAccountService
{
    Account GetAccount (string username, string password);
    Account GetAccountById (int id);
    Account GetStaffAccountById (int id);
    void RegisterAccount (Account account);
    void UpdateStaffAccount (Account account);
    void DeleteAccount (int id);
    bool CheckUsernameExisted (string username);
    bool CheckPhoneExisted (string phone);
    bool CheckEmailExisted (string email);
    List<Account> GetAllAccount ();
    List<Account> GetAllStaffAccount ();
    void AddStaffAccount (Account account);
}