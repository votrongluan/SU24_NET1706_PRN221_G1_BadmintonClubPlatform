using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace RazorWebApp.Pages.Admin
{
    public class AccountManageModel : AuthorPageServiceModel
    {
        private readonly IAccountService _service;

        public List<Account> StaffAccounts { get; set; }

        public AccountManageModel (IAccountService service)
        {
            _service = service;
        }

        public IActionResult OnGet ()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            StaffAccounts = _service.GetAllStaffAccount();

            return Page();
        }
    }
}
