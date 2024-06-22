using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
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
        public List<Account> FilterStaffAccounts { get; set; }
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

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private void Paging (string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0)
        {
            const int PageSize = 10;  // Set the number of items per page

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterStaffAccounts = searchProperty switch
                {
                    "AccountId" => StaffAccounts.Where(e => e.UserId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "Username" => StaffAccounts.Where(e => e.Username.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubId" => StaffAccounts.Where(e => e.ClubManageId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubName" => StaffAccounts.Where(e => e.ClubManage.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => StaffAccounts,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterStaffAccounts = sortProperty switch
                {
                    "AccountId" => sortOrder == -1 ? FilterStaffAccounts.OrderByDescending(e => e.UserId).ToList() : sortOrder == 1 ? FilterStaffAccounts.OrderBy(e => e.UserId).ToList() : FilterStaffAccounts,
                    "Username" => sortOrder == -1 ? FilterStaffAccounts.OrderByDescending(e => e.Username).ToList() : sortOrder == 1 ? FilterStaffAccounts.OrderBy(e => e.Username).ToList() : FilterStaffAccounts,
                    "ClubId" => sortOrder == -1 ? FilterStaffAccounts.OrderByDescending(e => e.ClubManageId).ToList() : sortOrder == 1 ? FilterStaffAccounts.OrderBy(e => e.ClubManageId).ToList() : FilterStaffAccounts,
                    "ClubName" => sortOrder == -1 ? FilterStaffAccounts.OrderByDescending(e => e.ClubManage.ClubName).ToList() : sortOrder == 1 ? FilterStaffAccounts.OrderBy(e => e.ClubManage.ClubName).ToList() : FilterStaffAccounts,
                    _ => FilterStaffAccounts,
                };
            }

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(FilterStaffAccounts.Count / (double)PageSize);
            FilterStaffAccounts = FilterStaffAccounts.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet (string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            InitialData();

            int page = Convert.ToInt32(Request.Query["page"]);
            Paging(searchString, searchProperty, sortProperty, sortOrder, page);

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
            TempData["SuccessMessage"] = "Thêm tài khoản thành công!";

            return RedirectToPage("./AccountManage");
        }

        public void InitialData ()
        {
            StaffAccounts = _service.AccountService.GetAllStaffAccount();
            FilterStaffAccounts = StaffAccounts;
            ViewData["ClubId"] = new SelectList(_service.ClubService.GetAllClubs().OrderBy(c => c.ClubId), "ClubId", "ClubName");
            string msg = TempData["SuccessMessage"] as string;
            if (msg != null)
            {
                SuccessMessage = msg;
            }
            else
            {
                SuccessMessage = string.Empty;
            }
        }
    }
}
