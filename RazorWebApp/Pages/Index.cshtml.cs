using BusinessObjects.Entities;
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
            InitializeData();
            return Page();
        }
    }
}
