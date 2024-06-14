using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class AccountRepository : IAccountRepository
{
    public List<Account> GetAllStaffAccount ()
    {
        return AccountDao.FindByCondition(e => e.Role.Equals(AccountRoleEnum.Staff.ToString())).ToList();
    }

    public Account GetAccountById (int id)
    {
        return AccountDao.FindByCondition(e => e.UserId == id).FirstOrDefault();
    }

    public Account GetAccount (string username, string password)
    {
        return AccountDao.FindByCondition((e => e.Username.Equals(username) && e.Password.Equals(password))).FirstOrDefault();
    }

    public void AddAccount (Account account)
    {
        AccountDao.Add(account);
    }

    public void UpdateAccount (Account account)
    {
        AccountDao.Update(account);
    }

    public void DeleteAccount (int id)
    {
        var account = GetAccountById(id);
        AccountDao.Delete(account);
    }
}