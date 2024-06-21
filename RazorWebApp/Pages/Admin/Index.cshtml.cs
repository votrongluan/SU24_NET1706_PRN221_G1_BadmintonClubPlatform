using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAppRazor.Pages.Admin
{
    public class IndexModel : AuthorPageServiceModel
    {
        public IActionResult OnGet()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here

            return Page();
        }
    }
}
