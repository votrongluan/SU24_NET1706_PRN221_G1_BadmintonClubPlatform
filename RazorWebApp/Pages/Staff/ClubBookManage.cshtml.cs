using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects.Dtos.Booking;
using WebAppRazor.Constants;
using Services.Service;

namespace WebAppRazor.Pages.Staff;

public class ClubBookManageModel : AuthorPageServiceModel
{
    private readonly IServiceManager serviceManager;

    public ClubBookManageModel(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [BindProperty]
    public BookOfflineRequestDto OfflineRequestDto { get; set; }
    public List<BookingViewModel> Bookings { get; set; }

    [BindProperty(SupportsGet = true)]
    public int TabIndex { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public int SelectedCourtId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SortBy { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SortOrder { get; set; }

    public List<Court> Courts { get; set; }
    public List<BookingDetail> BookingDetails { get; set; }
    public string Message { get; set; }

    public IActionResult OnGet()
    {
        try
        {
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

            // Validate club id
            if (LoginedAccount.ClubManageId == null)
            {
                return RedirectToPage("/NotFound");
            }

            int id = (int)LoginedAccount.ClubManageId;

            var isActiveClubById = serviceManager.ClubService.GetDeActiveClubById(id);

            if (isActiveClubById != null)
            {
                return RedirectToPage("/NotFound");
            }

            Bookings = GetBookings();

            int clubId = (int)LoginedAccount.ClubManageId;
            Courts = serviceManager.CourtService.GetCourtsByClubId(clubId);

            if (Request.Query.ContainsKey("selectedTabIndex"))
            {
                TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
            }

            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }

    private List<BookingViewModel> GetBookings()
    {
        var clubId = LoginedAccount.ClubManageId;
        var bookings = new List<BookingViewModel>();
        var bookingEntities = serviceManager.BookingService.GetAllBookings()
            .Where(x => x.ClubId == clubId && x.BookingTypeId != (int)BookingTypeEnum.LichThiDau);

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            bookingEntities = bookingEntities.Where(x => x.BookingId.ToString().Contains(SearchTerm));
        }

        if (bookingEntities == null || !bookingEntities.Any())
            return bookings;

        foreach (var booking in bookingEntities)
        {
            List<DateOnly?> dateList = new();
            var details = booking.BookingDetails.ElementAt(0);

            foreach (var detail in booking.BookingDetails)
            {
                dateList.Add(detail.BookDate);
            }

            bookings.Add(new BookingViewModel
            {
                Price = booking.TotalPrice ?? 0,
                BookingId = booking.BookingId,
                UserName = booking.User.Fullname,
                Service = booking.BookingType.Description,
                CourtId = details.CourtId.ToString(),
                UserPhone = booking.User.UserPhone,
                BookDates = dateList,
                StartTime = details.StartTime?.ToTimeSpan(),
                EndTime = details.EndTime?.ToTimeSpan(),
                PaymentStatus = booking.PaymentStatus == true ? "Đã thanh toán" : "Chưa thanh toán"
            });

        }

        return SortBookings(bookings);
    }

    private List<BookingViewModel> SortBookings(List<BookingViewModel> bookings)
    {
        if (string.IsNullOrEmpty(SortBy))
            return bookings;

        switch (SortBy)
        {
            case "BookingId":
                bookings = SortOrder == "desc" ? bookings.OrderByDescending(x => x.BookingId).ToList() : bookings.OrderBy(x => x.BookingId).ToList();
                break;
            case "StartTime":
                bookings = SortOrder == "desc" ? bookings.OrderByDescending(x => x.StartTime).ToList() : bookings.OrderBy(x => x.StartTime).ToList();
                break;
        }

        return bookings;
    }

    public IActionResult OnPostViewSlotByOrder()
    {
        try
        {
            if (SelectedDate != default && SelectedCourtId != 0)
            {
                BookingDetails = serviceManager.BookingDetailService.GetBookingsByDateAndCourt(SelectedDate, SelectedCourtId);
            }

            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage))
                return RedirectToPage(navigatePage);

            Bookings = GetBookings();

            int clubId = (int)LoginedAccount.ClubManageId;
            Courts = serviceManager.CourtService.GetCourtsByClubId(clubId);

            TabIndex = 2;

            if (Request.Query.ContainsKey("selectedTabIndex"))
            {
                TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
            }

            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }

    public IActionResult OnPostBookOffline()
    {
        try
        {
            LoadAccountFromSession();
            if (OfflineRequestDto.StartTime > OfflineRequestDto.EndTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Giờ bắt đầu không thể lớn hơn giờ kết thúc";
                return RedirectToPage("ClubBookManage");
            }

            var id = (int)LoginedAccount.ClubManageId;
            var bookClub = serviceManager.ClubService.GetClubById(id);

            if (OfflineRequestDto.StartTime < bookClub.OpenTime || OfflineRequestDto.EndTime > bookClub.CloseTime || OfflineRequestDto.EndTime < bookClub.OpenTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại khung giờ bạn chọn club chưa mở cửa";
                return RedirectToPage("ClubBookManage");
            }

            var result = serviceManager.BookingService.BookLichOffline(DateOnly.FromDateTime(DateTime.Today),
                OfflineRequestDto.StartTime, OfflineRequestDto.EndTime, OfflineRequestDto.CourtId,
                (int)LoginedAccount.ClubManageId, LoginedAccount.UserId);

            if (result.status)
            {
                TempData["Message"] = $"{MessagePrefix.SUCCESS}Đặt thành công với mã {result.bookId} và tổng tiền là {result.price}";
                return RedirectToPage("ClubBookManage");
            }
            else
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Không thể đặt do trùng giờ";
                return RedirectToPage("ClubBookManage");
            }
        }
        catch (Exception)
        {
            TempData["Message"] = $"{MessagePrefix.ERROR}Lỗi hệ thống";
            return RedirectToPage("ClubBookManage");
        }
    }

    public class BookingViewModel
    {
        public int Price { get; set; }
        public int BookingId { get; set; }
        public string UserName { get; set; }
        public string CourtId { get; set; }
        public string Service { get; set; }
        public string CourtType { get; set; }
        public string UserPhone { get; set; }
        public List<DateOnly?> BookDates { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string PaymentStatus { get; set; }
    }
}