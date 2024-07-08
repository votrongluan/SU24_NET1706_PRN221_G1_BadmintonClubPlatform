using BusinessObjects.Dtos.Booking;
using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class MatchUpdateModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public MatchCreateDto UpdatedMatch { get; set; } = new();
        public Match Match { get; set; }
        public List<CourtType> CourtTypes { get; set; }

        private void InitializeData(int id)
        {
            Match = _service.MatchService.GetMatchById(id);
            CourtTypes = _service.CourtTypeService.GetAllCourtTypes();
        }

        public MatchUpdateModel(IServiceManager service)
        {
            _service = service;
        }

        public IActionResult OnGet(int? id)
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Validate route id
            if (id == null) return RedirectToPage("/NotFound");

            InitializeData((int)id);

            if (Match == null) return RedirectToPage("/NotFound");

            if (Match?.Booking?.ClubId != LoginedAccount.ClubManageId) return RedirectToPage("/NotFound");

            var bookingDetail = Match.Booking.BookingDetails.ElementAt(0);
            UpdatedMatch.Title = Match.Title;
            UpdatedMatch.StartTime = (TimeOnly)bookingDetail.StartTime;
            UpdatedMatch.EndTime = (TimeOnly)bookingDetail.EndTime;
            UpdatedMatch.MatchDate = (DateOnly)bookingDetail.BookDate;
            UpdatedMatch.CourtTypeId = bookingDetail.Court.CourtTypeId;
            UpdatedMatch.Description = Match.Description;

            return Page();
        }

        public IActionResult OnPost(int id, int bookingId)
        {
            LoadAccountFromSession();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UpdatedMatch.StartTime > UpdatedMatch.EndTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Giờ bắt đầu không thể lớn hơn giờ kết thúc";
                return RedirectToPage("MatchManage");
            }

            try
            {
                _service.BookingService.DeleteBookingDetail(bookingId);

                BookingRequestDto dto = new()
                {
                    ClubId = (int)LoginedAccount.ClubManageId,
                    BookDate = UpdatedMatch.MatchDate,
                    StartTime = UpdatedMatch.StartTime,
                    EndTime = UpdatedMatch.EndTime,
                    UserId = (int)LoginedAccount.UserId,
                    BookingTypeId = (int)BookingTypeEnum.LichThiDau,
                    CourtTypeId = UpdatedMatch.CourtTypeId,
                    DefaultPrice = 0,
                };

                var result = _service.BookingService.BookLichThiDau(dto);

                if (!result.status)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Không thể đặt lịch thi đấu vào ngày {UpdatedMatch.MatchDate} từ {UpdatedMatch.StartTime} đến {UpdatedMatch.EndTime}";
                    return RedirectToPage("MatchManage");
                }

                var updatedMatch = new Match()
                {
                    MatchId = id,
                    BookingId = result.bookId,
                    Title = UpdatedMatch.Title,
                    Description = UpdatedMatch.Description,
                };

                _service.MatchService.UpdateMatch(updatedMatch);
                _service.BookingService.DeleteBooking(bookingId);

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
