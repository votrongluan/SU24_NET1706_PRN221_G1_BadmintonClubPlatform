using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using System.Diagnostics;
using System.Text.Json;

namespace RazorWebApp.Pages
{
    public class AuthenticationModel : PageModel
    {
        public readonly IServiceManager _service;
        [BindProperty] public AccountLoginDto AccountLogin { get; set; }
        public string Message { get; set; } = string.Empty;

        public AuthenticationModel (IServiceManager service)
        {
            _service = service;
        }

        public void OnGet ()
        {
        }

        public IActionResult OnPost ()
        {
            if (!ModelState.IsValid)
            {
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
                    Message = "Đăng nhập thất bại. Vui lòng kiểm tra thông tin đăng nhập và thử lại!";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return Page();
            }
        }
    }
}
