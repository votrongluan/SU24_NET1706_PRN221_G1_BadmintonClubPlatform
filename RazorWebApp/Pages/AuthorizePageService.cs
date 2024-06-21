using System.Text.Json;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages
{
    public class AuthorPageServiceModel : PageModel
    {
        public Account LoginedAccount { get; set; }

        protected void Logout()
        {
            HttpContext.Session.Clear();
        }

        protected void LoadAccountFromSession()
        {
            var accountJson = HttpContext.Session.GetString("Account");
            if (!string.IsNullOrEmpty(accountJson))
            {
                LoginedAccount = JsonSerializer.Deserialize<Account>(accountJson);
            }
        }

        protected string GetNavigatePageByAllowedRole(string allowedRole)
        {
            if (LoginedAccount == null) return "/Authentication";

            if (LoginedAccount.Role == allowedRole) return string.Empty;

            if (LoginedAccount.Role == AccountRoleEnum.Admin.ToString()) return "/Admin/Index";

            if (LoginedAccount.Role == AccountRoleEnum.Staff.ToString()) return "/Staff/Index";

            return "/NotFound";
        }

        public IActionResult OnGetLogout()
        {
            Logout();

            return RedirectToPage("/Authentication");
        }
    }
}