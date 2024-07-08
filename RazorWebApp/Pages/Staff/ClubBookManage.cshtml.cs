using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAppRazor.Pages.Staff
{
    public class ClubBookManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager serviceManager;

        public ClubBookManageModel(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public List<BookingViewModel> Bookings { get; set; }

        public IActionResult OnGet()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage))
                return RedirectToPage(navigatePage);

            // Code go from here
            Bookings = GetBookings();
            return Page();
        }

        private List<BookingViewModel> GetBookings()
        {
            var clubId = LoginedAccount.ClubManageId;
            var bookings = new List<BookingViewModel>();
            var bookingEntities = serviceManager.BookingService.GetAllBookingsWithBookingDetails().Where(x => x.ClubId == clubId);

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
                        CourtType = booking.BookingType.Description,
                        UserPhone = booking.User.UserPhone,
                        BookDate = detail.BookDate?.ToDateTime(new TimeOnly(0, 0)),
                        StartTime = detail.StartTime?.ToTimeSpan(),
                        EndTime = detail.EndTime?.ToTimeSpan(),
                        PaymentStatus = booking.PaymentStatus == true ? "Đã thanh toán" : "Chưa thanh toán"
                    }); ;
                }
            }
            return bookings;
        }

        public class BookingViewModel
        {
            public int BookingId { get; set; }
            public string UserName { get; set; }
            public string CourtType { get; set; }
            public string UserPhone { get; set; }
            public DateTime? BookDate { get; set; }
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public string PaymentStatus { get; set; }
        }
    }
}