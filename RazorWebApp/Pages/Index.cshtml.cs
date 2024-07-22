using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;

namespace WebAppRazor.Pages
{
    public class IndexModel : AuthorPageServiceModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IServiceManager _service;
        public List<Club> MostRating { get; set; }
        public List<Club> MostBooking { get; set; }
        public List<Club> MostPopular { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IServiceManager service)
        {
            _logger = logger;
            _service = service;
        }
        public void InitializeData()
        {
            MostRating = _service.ClubService.GetMostRatingClubs();
            MostBooking = _service.ClubService.GetMostBookingClubs();
            MostPopular = _service.ClubService.GetMostPopularClubs();
        }

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

                InitializeData();

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
