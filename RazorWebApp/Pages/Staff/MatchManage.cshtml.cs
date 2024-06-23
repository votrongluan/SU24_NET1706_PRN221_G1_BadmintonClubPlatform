using BusinessObjects.Dtos.Club;
using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Services.IService;

namespace WebAppRazor.Pages.Staff
{
    public class MatchManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public MatchCreateDto CreatedClub { get; set; }
        public List<Match> Matches { get; set; }
        public List<BookingType> BookingTypes { get; set; }
        public List<Slot> Slots { get; set; }
        public List<Match> FilterMatches { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // MESSAGE FOR ACTION
        public string Message { get; set; }

        private void InitializeData()
        {
            Matches = _service.MatchService.GetAllMatches();
            BookingTypes = _service.BookingTypeService.GetAllBookingTypes();
            Slots = _service.SlotService.GetAllSlot();

            FilterMatches = Matches;
        }

        public MatchManageModel(IServiceManager service)
        {
            _service = service;
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0)
        {
            const int PageSize = 10;  // Set the number of items per page

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterMatches = searchProperty switch
                {
                    //"ClubId" => Matches.Where(e => e.ClubId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    //"ClubName" => Matches.Where(e => e.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    //"Address" => Matches.Where(e => e.Address.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    //"ClubPhone" => Matches.Where(e => e.ClubPhone.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => FilterMatches,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterMatches = sortProperty switch
                {
                    //"ClubId" => sortOrder == -1 ? FilterMatches.OrderByDescending(e => e.ClubId).ToList() : sortOrder == 1 ? FilterMatches.OrderBy(e => e.ClubId).ToList() : FilterMatches,
                    //"ClubName" => sortOrder == -1 ? FilterMatches.OrderByDescending(e => e.ClubName).ToList() : sortOrder == 1 ? FilterMatches.OrderBy(e => e.ClubName).ToList() : FilterMatches,
                    //"Address" => sortOrder == -1 ? FilterMatches.OrderByDescending(e => e.Address).ToList() : sortOrder == 1 ? FilterMatches.OrderBy(e => e.Address).ToList() : FilterMatches,
                    //"ClubPhone" => sortOrder == -1 ? FilterMatches.OrderByDescending(e => e.ClubPhone).ToList() : sortOrder == 1 ? FilterMatches.OrderBy(e => e.ClubPhone).ToList() : FilterMatches,
                    _ => FilterMatches,
                };
            }

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(FilterMatches.Count / (double)PageSize);
            FilterMatches = FilterMatches.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Set and clear the message
            if (!string.IsNullOrWhiteSpace(Message))
            {
                Message = string.Empty;
            }

            if (TempData.ContainsKey("Message"))
            {
                Message = TempData["Message"].ToString();
            }

            InitializeData();

            int page = Convert.ToInt32(Request.Query["page"]);
            Paging(searchString, searchProperty, sortProperty, sortOrder, page);

            return Page();
        }
    }
}
