using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IAccountRepository
{
    List<Account> GetAllStaffAccount();
    Account GetAccountById(int id);
    Account GetAccount(string username, string password);
    void AddAccount(Account account);
    void UpdateAccount(Account account);
    void DeleteAccount(int id);
}