using BusinessObjects.Dtos.Booking;
using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff;

public class MatchDeleteModel : AuthorPageServiceModel
{
    private readonly IServiceManager _service;
    [BindProperty] public MatchCreateDto DeletedMatch { get; set; } = new();
    public Match Match { get; set; }
    public List<CourtType> CourtTypes { get; set; }
    public string CourtName { get; set; }

    private void InitializeData(int id)
    {
        Match = _service.MatchService.GetMatchById(id);
        CourtTypes = _service.CourtTypeService.GetAllCourtTypes();
        CourtName = CourtTypes
            .Where(e => e.CourtTypeId == Match.Booking.BookingDetails.ElementAt(0).Court.CourtTypeId)
            .FirstOrDefault().TypeName;
    }

    public MatchDeleteModel(IServiceManager service)
    {
        _service = service;
    }

    public IActionResult OnGet(int? id)
    {
        try
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Validate route id
            if (id == null) return RedirectToPage("/NotFound");

            // Validate club id is active
            //-------------------------------
            if (LoginedAccount.ClubManageId == null)
            {
                return RedirectToPage("/NotFound");
            }

            int validateClubId = (int)LoginedAccount.ClubManageId;

            var isActiveClubById = _service.ClubService.GetDeActiveClubById(validateClubId);

            if (isActiveClubById != null)
            {
                return RedirectToPage("/NotFound");
            }
            //-------------------------------
            // End of validate club is active

            InitializeData((int)id);

            if (Match == null) return RedirectToPage("/NotFound");

            if (Match?.Booking?.ClubId != LoginedAccount.ClubManageId) return RedirectToPage("/NotFound");

            var bookingDetail = Match.Booking.BookingDetails.ElementAt(0);
            DeletedMatch.Title = Match.Title;
            DeletedMatch.StartTime = (TimeOnly)bookingDetail.StartTime;
            DeletedMatch.EndTime = (TimeOnly)bookingDetail.EndTime;
            DeletedMatch.MatchDate = (DateOnly)bookingDetail.BookDate;
            DeletedMatch.CourtTypeId = bookingDetail.Court.CourtTypeId;
            DeletedMatch.Description = Match.Description;

            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }

    public IActionResult OnPost(int id, int bookingId)
    {
        try
        {
            LoadAccountFromSession();
            _service.BookingService.DeleteBookingDetail(bookingId);
            _service.MatchService.DeleteMatch(id);
            _service.BookingService.DeleteBooking(bookingId);

            TempData["Message"] = $"{MessagePrefix.SUCCESS}Lịch thi đấu đã được xóa thành công";
            return RedirectToPage("MatchManage");
        }
        catch
        {
            TempData["Message"] = $"{MessagePrefix.ERROR}Lỗi từ phía hệ thống";
            return RedirectToPage("MatchManage");
        }
    }
}