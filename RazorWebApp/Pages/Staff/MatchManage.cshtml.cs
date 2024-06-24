using BusinessObjects.Dtos.Club;
using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class MatchManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public MatchCreateDto CreatedClub { get; set; }
        public List<Match> Matches { get; set; }
        public List<MatchResponseDto> MatchesDto { get; set; }
        public List<MatchResponseDto> FilterMatchesDto { get; set; }
        public List<BookingType> BookingTypes { get; set; }
        public List<Slot> Slots { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // MESSAGE FOR ACTION
        public string Message { get; set; }

        private void InitializeData()
        {
            Matches = _service.MatchService.GetAllMatches();
            MatchesDto = Matches.Select(e => e.ToMatch()).ToList();
            BookingTypes = _service.BookingTypeService.GetAllBookingTypes();
            Slots = _service.SlotService.GetAllSlot();

            FilterMatchesDto = MatchesDto;
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
                FilterMatchesDto = searchProperty switch
                {
                    "Title" => MatchesDto.Where(e => e.Title.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "Description" => MatchesDto.Where(e => e.Description.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "MatchDate" => MatchesDto.Where(e => e.MatchDate.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "MatchTime" => MatchesDto.Where(e => e.MatchTime.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "CourtId" => MatchesDto.Where(e => e.CourtId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => MatchesDto,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterMatchesDto = sortProperty switch
                {
                    "ClubId" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.Title).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.Title).ToList() : FilterMatchesDto,
                    "ClubName" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.Description).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.Description).ToList() : FilterMatchesDto,
                    "Address" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.MatchDate).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.MatchDate).ToList() : FilterMatchesDto,
                    "ClubPhone" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.MatchTime).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.MatchTime).ToList() : FilterMatchesDto,
                    _ => FilterMatchesDto,
                };
            }

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(FilterMatchesDto.Count / (double)PageSize);
            FilterMatchesDto = FilterMatchesDto.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
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
