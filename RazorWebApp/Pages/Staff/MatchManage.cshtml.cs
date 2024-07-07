using BusinessObjects.Dtos.Booking;
using BusinessObjects.Dtos.Club;
using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class MatchManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public MatchCreateDto CreatedMatch { get; set; }
        public List<Match> Matches { get; set; }
        public List<MatchResponseDto> MatchesDto { get; set; }
        public List<MatchResponseDto> FilterMatchesDto { get; set; }
        public List<CourtType> CourtTypes { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // MESSAGE FOR ACTION
        public string Message { get; set; }
        public int TotalFindMatch { get; set; }

        private void InitializeData()
        {
            Matches = _service.MatchService.GetAllMatches();
            MatchesDto = Matches.Select(e => e.ToMatchResponseDto()).ToList();
            CourtTypes = _service.CourtTypeService.GetAllCourtTypes();

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
                    "Title" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.Title).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.Title).ToList() : FilterMatchesDto,
                    "Description" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.Description).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.Description).ToList() : FilterMatchesDto,
                    "MatchDate" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.MatchDateOnly).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.MatchDateOnly).ToList() : FilterMatchesDto,
                    "MatchTime" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.MatchTime).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.MatchTime).ToList() : FilterMatchesDto,
                    "CourtId" => sortOrder == -1 ? FilterMatchesDto.OrderByDescending(e => e.CourtId).ToList() : sortOrder == 1 ? FilterMatchesDto.OrderBy(e => e.CourtId).ToList() : FilterMatchesDto,
                    _ => FilterMatchesDto,
                };
            }

            TotalFindMatch = FilterMatchesDto?.Count ?? 0;

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

        public IActionResult OnPost()
        {
            LoadAccountFromSession();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CreatedMatch.StartTime > CreatedMatch.EndTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Giờ bắt đầu không thể lớn hơn giờ kết thúc";
                return RedirectToPage("MatchManage");
            }

            try
            {
                BookingRequestDto dto = new()
                {
                    ClubId = (int)LoginedAccount.ClubManageId,
                    BookDate = CreatedMatch.MatchDate,
                    StartTime = CreatedMatch.StartTime,
                    EndTime = CreatedMatch.EndTime,
                    UserId = (int)LoginedAccount.UserId,
                    BookingTypeId = (int)BookingTypeEnum.LichThiDau,
                    CourtTypeId = CreatedMatch.CourtTypeId,
                    DefaultPrice = 0,
                };

                var result = _service.BookingService.BookLichThiDau(dto);

                if (!result.status)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Không thể đặt lịch thi đấu vào ngày {CreatedMatch.MatchDate} từ {CreatedMatch.StartTime} đến {CreatedMatch.EndTime}";
                    return RedirectToPage("MatchManage");
                }

                _service.MatchService.AddMatch(new Match()
                {
                    Description = CreatedMatch.Description,
                    Title = CreatedMatch.Title,
                    BookingId = result.bookId,
                });

                TempData["Message"] = $"{MessagePrefix.SUCCESS}Lịch thi đấu đã được đặt thành công";
                return RedirectToPage("MatchManage");
            }
            catch
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Lỗi từ phía hệ thống";
                return RedirectToPage("MatchManage");
            }
        }
    }
}
