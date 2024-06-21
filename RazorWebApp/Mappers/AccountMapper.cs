using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers
{
    public static class AccountMapper
    {
        public static AccountResponseDto ToResponseAccountDto (this Account e)
        {
            return new AccountResponseDto()
            {
                AccountId = e.UserId,
                Username = e.Username,
                Password = e.Password,
                Phone = e.UserPhone,
                Email = e.Email,
            };
        }

        public static Account ToAccount (this AccountRegisterDto e)
        {
            return new Account()
            {
                Username = e.Username,
                Password = e.Password,
                UserPhone = e.UserPhone,
                Email = e.Email,
            };
        }

        public static Account ToAccount (this AccountAddDto e)
        {
            return new Account()
            {
                Username = e.Username,
                Password = e.Password,
                ClubManageId = e.ClubId
            };
        }
    }
}
