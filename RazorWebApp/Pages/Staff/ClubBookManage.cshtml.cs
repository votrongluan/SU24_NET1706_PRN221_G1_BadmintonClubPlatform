using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects.Dtos.Booking;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff
{
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

        public List<Court> Courts { get; set; }
        public List<BookingDetail> BookingDetails { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
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

            Bookings = GetBookings();

            int clubId = (int)LoginedAccount.ClubManageId;
            Courts = serviceManager.CourtService.GetCourtsByClubId(clubId);

            if (Request.Query.ContainsKey("selectedTabIndex"))
            {
                TabIndex = int.Parse(Request.Query["selectedTabIndex"]);
            }

            return Page();
        }

        private List<BookingViewModel> GetBookings()
        {
            var clubId = LoginedAccount.ClubManageId;
            var bookings = new List<BookingViewModel>();
            var bookingEntities = serviceManager.BookingService.GetAllBookingsWithBookingDetails()
                .Where(x => x.ClubId == clubId);

            if (bookingEntities == null || !bookingEntities.Any())
                return bookings;

            foreach (var booking in bookingEntities)
            {
                foreach (var detail in booking.BookingDetails)
                {
                    bookings.Add(new BookingViewModel
                    {
                        BookingId = booking.BookingId,
                        UserName = booking.User.Fullname,
                        Service = booking.BookingType.Description,
                        CourtId = detail.CourtId.ToString(),
                        UserPhone = booking.User.UserPhone,
                        BookDate = detail.BookDate?.ToDateTime(new TimeOnly(0, 0)),
                        StartTime = detail.StartTime?.ToTimeSpan(),
                        EndTime = detail.EndTime?.ToTimeSpan(),
                        PaymentStatus = booking.PaymentStatus == true ? "Đã thanh toán" : "Chưa thanh toán"
                    });
                }
            }
            return bookings;
        }

        public IActionResult OnPostViewSlotByOrder()
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

        public IActionResult OnPostBookOffline()
        {
            LoadAccountFromSession();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (OfflineRequestDto.StartTime > OfflineRequestDto.EndTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Giờ bắt đầu không thể lớn hơn giờ kết thúc";
                return RedirectToPage("ClubBookManage");
            }

            try
            {
                var result = serviceManager.BookingService.BookLichOffline(DateOnly.FromDateTime(DateTime.Today),
                    OfflineRequestDto.StartTime, OfflineRequestDto.EndTime, OfflineRequestDto.CourtId,
                    (int)LoginedAccount.ClubManageId, LoginedAccount.UserId);

                if (result.status)
                {
                    TempData["Message"] = $"{MessagePrefix.SUCCESS}Đặt thành công";
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
            public int BookingId { get; set; }
            public string UserName { get; set; }
            public string CourtId { get; set; }
            public string Service { get; set; }
            public string CourtType { get; set; }
            public string UserPhone { get; set; }
            public DateTime? BookDate { get; set; }
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public string PaymentStatus { get; set; }
        }
    }
}
