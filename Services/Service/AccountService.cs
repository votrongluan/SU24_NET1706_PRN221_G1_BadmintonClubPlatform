using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class AccountService : IAccountService
{
    private readonly IRepositoryManager _repo;

    public AccountService (IRepositoryManager repo)
    {
        _repo = repo;
    }

    public Account GetAccount (string username, string password)
    {
        return _repo.Account.GetAccount(username, password);
    }

    public void RegisterAccount (Account account)
    {
        _repo.Account.AddAccount(account);
    }

    public bool CheckUsernameExisted (string username)
    {
        return _repo.Account.CheckUsernameExisted(username);
    }

    public bool CheckPhoneExisted (string phone)
    {
        return _repo.Account.CheckPhoneExisted(phone);
    }

    public bool CheckEmailExisted (string email)
    {
        return _repo.Account.CheckEmailExisted(email);
    }

    public List<Account> GetAllAccount ()
    {
        return _repo.Account.GetAllAccount();
    }

    public List<Account> GetAllStaffAccount ()
    {
        return _repo.Account.GetAllStaffAccount();
    }

    public void AddStaffAccount (Account account)
    {
        _repo.Account.AddStaffAccount(account);
    }
}