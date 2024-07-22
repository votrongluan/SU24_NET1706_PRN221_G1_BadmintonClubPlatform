using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services.IService;
using Services.Service;
using WebAppRazor.Constants;
using System.Text.RegularExpressions;

namespace WebAppRazor.Pages.Customer
{
    public class AccountInfoModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _serviceManager;

        public AccountInfoModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public string Message { get; set; }

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty(SupportsGet = true)]
        public int TabIndex { get; set; } = 1;

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string InputAgainNewPassword { get; set; }

        [BindProperty]
        public string InputNewPassword { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                LoadAccountFromSession();
                var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

                if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

                if (!string.IsNullOrWhiteSpace(Message))
                {
                    Message = string.Empty;
                }

                if (TempData.ContainsKey("Message"))
                {
                    Message = TempData["Message"].ToString();
                }

                Account = _serviceManager.AccountService.GetAccountById(LoginedAccount.UserId);
                if (Request.Query.ContainsKey("selectedTabIndex"))
                {
                    TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
                }

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                LoadAccountFromSession();

                if (!ValidatePhoneNumber(Account.UserPhone))
                {
                    Message = $"{MessagePrefix.ERROR} Số điện thoại không hợp lệ.";
                    return Page();
                }

                if (!ValidateEmail(Account.Email))
                {
                    Message = $"{MessagePrefix.ERROR} Email không hợp lệ.";
                    return Page();
                }

                Account.Password = LoginedAccount.Password;
                Account.Username = LoginedAccount.Username;
                Account.Role = LoginedAccount.Role;

                _serviceManager.AccountService.UpdateAccount(Account);

                UpdateAccountSession(Account);

                TabIndex = 1;

                var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

                if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

                Account = _serviceManager.AccountService.GetAccountById(LoginedAccount.UserId);
                if (Request.Query.ContainsKey("selectedTabIndex"))
                {
                    TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
                }

                TempData["Message"] = $"{MessagePrefix.SUCCESS}Thông tin cá nhân đã cập nhật thành công.";

                return RedirectToPage("./AccountInfo");
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            string pattern = @"^(0[3|5|7|8|9])+([0-9]{8})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult OnPostUpdatePassword()
        {
            try
            {
                LoadAccountFromSession();
                TabIndex = 2;
                Account = _serviceManager.AccountService.GetAccountById(LoginedAccount.UserId);

                if (OldPassword.Equals(Account.Password))
                {
                    if (InputNewPassword.Equals(InputAgainNewPassword))
                    {
                        Account.Password = InputNewPassword;
                        Account.Username = LoginedAccount.Username;
                        Account.Fullname = LoginedAccount.Fullname;
                        Account.Email = LoginedAccount.Email;
                        Account.UserPhone = LoginedAccount.UserPhone;
                        Account.Role = LoginedAccount.Role;

                        _serviceManager.AccountService.UpdateAccount(Account);
                    }
                    else
                    {
                        Message = $"{MessagePrefix.ERROR} Mật khẩu mới không giống nhau.";
                        return Page();
                    }
                }
                else
                {
                    Message = $"{MessagePrefix.ERROR} Mật khẩu cũ không đúng.";
                    return Page();
                }
                LoadAccountFromSession();
                var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

                if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);
                Account = _serviceManager.AccountService.GetAccountById(LoginedAccount.UserId);
                if (Request.Query.ContainsKey("selectedTabIndex"))
                {
                    TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
                }
                TempData["Message"] = $"{MessagePrefix.SUCCESS}Mật khẩu đã cập nhật thành công.";

                if (TempData.ContainsKey("Message"))
                {
                    Message = TempData["Message"].ToString();
                }

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
