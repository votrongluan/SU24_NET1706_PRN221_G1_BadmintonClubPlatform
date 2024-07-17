using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace WebAppRazor.Pages.Staff
{
    public class IndexModel : AuthorPageServiceModel
    {

        private readonly IServiceManager _serviceManager;

        public IndexModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [BindProperty]
        public Club Club { get; set; }

        public IActionResult OnGet()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            if (LoginedAccount.ClubManageId != null)
            {
                Club = _serviceManager.ClubService.GetClubById((int)LoginedAccount.ClubManageId);

            }    

                return Page();
        }
    }
}
