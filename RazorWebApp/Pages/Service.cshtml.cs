using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAppRazor.Pages
{
    public class ServiceModel : AuthorPageServiceModel
    {
        public IActionResult OnGet()
        {
            try
            {
                LoadAccountFromSession();

                if (LoginedAccount != null)
                {
                    var role = (string)LoginedAccount.Role;
                    if (role == AccountRoleEnum.Admin.ToString()) return RedirectToPage("/Admin/Index");
                    if (role == AccountRoleEnum.Staff.ToString()) return RedirectToPage("/Staff/Index");
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
