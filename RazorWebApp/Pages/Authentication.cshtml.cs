using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RazorWebApp.Mappers;
using Services.IService;
using System.Diagnostics;
using System.Text.Json;

namespace RazorWebApp.Pages
{
    public class AuthenticationModel : PageModel
    {
        public readonly IServiceManager _service;

        public AccountLoginDto AccountLogin { get; set; }

        public AccountRegisterDto AccountRegister { get; set; }
        public List<string> Message { get; set; } = new List<string>();
        public int TabIndex { get; set; }

        public AuthenticationModel (IServiceManager service)
        {
            _service = service;
            TabIndex = 1;
        }

        public void OnGet ()
        {
        }

        public IActionResult OnPostLogin ([Bind("UserName, Password")] AccountLoginDto AccountLogin)
        {
            if (!ModelState.IsValid)
            {
                TabIndex = 1;
                // Return to page with errors
                return Page();
            }

            // Get account from database
            var result = _service.AccountService.GetAccount(AccountLogin.UserName, AccountLogin.Password);
            try
            {
                if (result != null)
                {
                    // Serialize the result to a JSON string and put into session
                    HttpContext.Session.SetString("Account", JsonSerializer.Serialize(result));
                    return RedirectToPage("/Index");
                }
                else
                {
                    Message.Add("Tên đăng nhập hoặc mật khẩu không đúng!");
                }
            }
            catch (Exception ex)
            {
                Message.Add(ex.Message);
            }

            TabIndex = 1;
            return Page();
        }

        public IActionResult OnPostRegister ([Bind("Username, Password, ConfirmPassword, UserPhone, Email")] AccountRegisterDto AccountRegister)
        {
            // Check if model state is valid
            if (!ModelState.IsValid)
            {
                TabIndex = 2;
                return Page();
            }

            try
            {
                List<bool> checkingConditin = new List<bool>
                {
                    _service.AccountService.CheckUsernameExisted(AccountRegister.Username),
                    AccountRegister.ConfirmPassword == AccountRegister.Password,
                    _service.AccountService.CheckPhoneExisted(AccountRegister.UserPhone),
                    _service.AccountService.CheckEmailExisted(AccountRegister.Email)
                };

                // If any checking conditions fail, add appropriate error messages and return page
                if (checkingConditin.Contains(false))
                {
                    if (checkingConditin[0])
                    {
                        Message.Add("Tên đăng nhập đã tồn tại!");

                    }
                    if (checkingConditin[1])
                    {
                        Message.Add("Mật khẩu không khớp!");
                    }
                    if (checkingConditin[2])
                    {
                        Message.Add("Số điện thoại đã tồn tại!");
                    }
                    if (!checkingConditin[3])
                    {
                        Message.Add("Email đã tồn tại!");
                    }
                    TabIndex = 2;
                    return Page();
                }

                // Add new account to database
                _service.AccountService.RegisterAccount(AccountRegister.ToAccount());
                Message.Add("Đăng ký tài khoản thành công!");
            }
            catch (Exception ex)
            {
                Message.Add(ex.Message); // Set the exception message to Message
                TabIndex = 2;
                return Page();
            }

            TabIndex = 2;
            return Page();
        }
    }
}
