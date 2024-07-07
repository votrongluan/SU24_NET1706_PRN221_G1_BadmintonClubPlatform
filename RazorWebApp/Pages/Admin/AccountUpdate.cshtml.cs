using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using System.Net.WebSockets;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class AccountUpdateModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;

        [BindProperty]
        public AccountUpdateDto AccountUpdate { get; set; }

        public Account BaseAccount { get; set; }

        public AccountUpdateModel (IServiceManager service)
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

            AccountUpdate = _service.AccountService.GetStaffAccountById(id.Value).ToAccountUpdate();
            BaseAccount = _service.AccountService.GetStaffAccountById(id.Value);
            ViewData["ClubId"] = new SelectList(_service.ClubService.GetAllClubs().OrderBy(c => c.ClubId), "ClubId", "ClubName");

            return Page();
        }

        public IActionResult OnPost (int? id)
        {
            BaseAccount = _service.AccountService.GetStaffAccountById(id.Value);
            if (!ModelState.IsValid)
            {
                ViewData["ClubId"] = new SelectList(_service.ClubService.GetAllClubs().OrderBy(c => c.ClubId), "ClubId", "ClubName");
                return Page();
            }

            // SET VALUE FOR EACH FIELD OF ACCOUNTUPDATE OBJECT
            AccountUpdate.UserId = BaseAccount.UserId;
            AccountUpdate.Role = BaseAccount.Role;
            AccountUpdate.Email = BaseAccount.Email;
            AccountUpdate.UserPhone = BaseAccount.UserPhone;
            _service.AccountService.UpdateStaffAccount(AccountUpdate.ToAccount());
            TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";

            return RedirectToPage("./AccountManage");
        }
    }
}
