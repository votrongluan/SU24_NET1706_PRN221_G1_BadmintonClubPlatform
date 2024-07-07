using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace WebAppRazor.Pages.Admin
{
    public class AccountDeleteModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        public Account Account { get; set; }

        public AccountDeleteModel (IServiceManager service)
        {
            _service = service;
        }
        public IActionResult OnGet (int? id)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            Account = _service.AccountService.GetStaffAccountById(id.Value);

            return Page();
        }

        public IActionResult OnPost (int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            _service.AccountService.DeleteAccount(id.Value);
            TempData["SuccessMessage"] = "Xoá tài khoản thành công!";

            return RedirectToPage("./AccountManage");
        }
    }
}
