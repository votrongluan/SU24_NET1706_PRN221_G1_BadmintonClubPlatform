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

        // CONVERT FROM ACCOUNT TO ACCOUNT UPDATE
        public static Account ToAccount (this AccountUpdateDto e)
        {
            return new Account()
            {
                UserId = e.UserId,
                Username = e.Username,
                Password = e.Password,
                UserPhone = e.UserPhone,
                Email = e.Email,
                Role = e.Role,
                Fullname = e.Fullname,
                ClubManageId = e.ClubId
            };
        }

        // GETTING ACCOUNT UPDATE CONVERT TO ACCOUNT
        public static AccountUpdateDto ToAccountUpdate (this Account e)
        {
            return new AccountUpdateDto()
            {
                UserId = e.UserId,
                Email = e.Email,
                Fullname = e.Fullname,
                UserPhone = e.UserPhone,
                Role = e.Role,
                Username = e.Username,
                Password = e.Password,
                ClubId = e.ClubManageId
            };
        }
    }
}
