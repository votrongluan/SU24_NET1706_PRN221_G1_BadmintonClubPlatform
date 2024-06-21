using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class AccountManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;

        public List<Account> StaffAccounts { get; set; }
        public List<Club> Clubs { get; set; }
        public bool ShowAddAccountModal { get; set; }

        [BindProperty]
        public AccountAddDto AddAccount { get; set; }

        public AccountManageModel (IServiceManager service)
        {
            _service = service;
        }

        // MESSAGE FOR ACTION
        public List<string> ErrorMessage = new List<string>();
        public string SuccessMessage { get; set; }

        public IActionResult OnGet ()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            InitialData();

            return Page();
        }

        // POST FOR ADD ACCOUNT
        public IActionResult OnPostAddAccount ()
        {
            InitialData();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            List<bool> checkingCondition = new List<bool>
                {
                    _service.AccountService.CheckUsernameExisted(AddAccount.Username),
                };

            if (checkingCondition.Any(c => c == true))
            {
                ErrorMessage.Add("Tên tài khoản đã tồn tại");
                ShowAddAccountModal = true;
                return Page();
            }
            _service.AccountService.AddStaffAccount(AddAccount.ToAccount());

            return RedirectToPage("./AccountManage", new { SuccessMessage = "Thêm tài khoản nhân viên thành công" });
        }

        public void InitialData ()
        {
            StaffAccounts = _service.AccountService.GetAllStaffAccount();
            ViewData["ClubId"] = new SelectList(_service.ClubService.GetAllClubs().OrderBy(c => c.ClubId), "ClubId", "ClubName");
            string msg = Request.Query["SuccessMessage"];
            if (msg != null)
            {
                SuccessMessage = msg;
            }
        }
    }
}
