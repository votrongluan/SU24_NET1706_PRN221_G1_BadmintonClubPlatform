using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAppRazor.Pages.Staff
{
    public class CourtManageModel : AuthorPageServiceModel
    {
        public IActionResult OnGet()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here

            return Page();
        }
    }
}
