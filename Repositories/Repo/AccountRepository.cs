using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class AccountRepository : IAccountRepository
{
    public List<Account> GetAllStaffAccount ()
    {
        return AccountDao.FindByCondition(e => e.Role.Equals(AccountRoleEnum.Staff.ToString()))
            .Include(e => e.ClubManage).ToList();
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

    public bool CheckUsernameExisted (string username)
    {
        return AccountDao.FindByCondition(e => e.Username.Equals(username)).Any();
    }

    public bool CheckPhoneExisted (string phone)
    {
        return AccountDao.FindByCondition(e => e.UserPhone.Equals(phone)).Any();
    }

    public bool CheckEmailExisted (string email)
    {
        return AccountDao.FindByCondition(e => e.Email.Equals(email)).Any();
    }

    public List<Account> GetAllAccount ()
    {
        return AccountDao.GetAll().ToList();
    }
}