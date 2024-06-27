using BusinessObjects.Dtos.Booking;
using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Customer
{
    public class BookModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;

        public BookModel(IServiceManager service)
        {
            _service = service;
        }

        [BindProperty] public BookingRequestDto BookingRequestDto { get; set; }
        public string Message { get; set; }
        public int ClubId { get; set; }
        public ResponseClubDto Club { get; set; }
        public List<CourtType> CourtTypes { get; set; }
        public List<Slot> Slots { get; set; }
        public List<BookingType> BookingTypes { get; set; }

        public void InitializeData()
        {
            Club = _service.ClubService.GetClubById(ClubId).ToResponseClubDto();
            CourtTypes = _service.CourtTypeService.GetAllCourtTypes();
            Slots = _service.SlotService.GetAllSlot().Where(e => e.ClubId == ClubId).ToList();
            BookingTypes = _service.BookingTypeService.GetAllBookingTypes().Where(e => e.BookingTypeId != (int)BookingTypeEnum.LichThiDau).ToList();
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

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

            ClubId = (int)id;
            InitializeData();

            return Page();
        }

        public IActionResult OnPost(int id, int slotId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LoadAccountFromSession();

            if (BookingRequestDto.BookDate < DateOnly.FromDateTime(DateTime.Now.AddDays(2)) ||
                BookingRequestDto.BookDate > DateOnly.FromDateTime(DateTime.Now.AddDays(14)))
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Ngày chưa chính xác, phải lớn hơn hôm nay 2 ngày và không quá 2 tuần";
                return RedirectToPage("Book", new { id });
            }

            try
            {
                if (BookingRequestDto.BookingTypeId == (int)BookingTypeEnum.LichNgay)
                {
                    var availableCourt = _service.CourtService.GetAllCourts();
                    var bookingDetailInSlotAndDate = _service.BookingDetailService.GetAllBookingDetails().Where(e => e.BookDate == BookingRequestDto.BookDate && e.Court.CourtTypeId == BookingRequestDto.CourtTypeId);

                    availableCourt = availableCourt.Where(e => !bookingDetailInSlotAndDate.Any(ee => ee.CourtId == e.CourtId)).ToList();

                    if (availableCourt.Any())
                    {
                        Booking booking = new()
                        {
                            BookingTypeId = BookingRequestDto.BookingTypeId,
                            ClubId = id,
                            PaymentStatus = false,
                            UserId = LoginedAccount.UserId,
                        };

                        _service.BookingService.AddBooking(booking);

                        BookingDetail bookingDetail = new()
                        {
                            BookingId = booking.BookingId,
                            BookDate = BookingRequestDto.BookDate,
                            CourtId = availableCourt.ElementAt(0).CourtId,
                            //SlotId = slotId,
                        };

                        _service.BookingDetailService.AddBookingDetail(bookingDetail);

                        TempData["Message"] = $"{MessagePrefix.SUCCESS}Đặt thành công";
                        return RedirectToPage("Book", new { id });
                    }
                    else
                    {
                        TempData["Message"] = $"{MessagePrefix.WARNING}Câu lạc bộ không còn sân trống";
                        return RedirectToPage("Book", new { id });
                    }
                }

                if (BookingRequestDto.BookingTypeId == (int)BookingTypeEnum.LichCoDinh)
                {
                    var availableCourt = _service.CourtService.GetAllCourts();
                    var date = BookingRequestDto.BookDate;

                    for (int i = 1; i <= BookingRequestDto.WeekCount; i++)
                    {
                        var bookingDetailInSlotAndDate = _service.BookingDetailService.GetAllBookingDetails().Where(e => e.BookDate == date && e.Court.CourtTypeId == BookingRequestDto.CourtTypeId);

                        availableCourt = availableCourt.Where(e => !bookingDetailInSlotAndDate.Any(ee => ee.CourtId == e.CourtId)).ToList();

                        date = date.AddDays(7);
                    }

                    if (availableCourt.Any())
                    {
                        Booking booking = new()
                        {
                            BookingTypeId = BookingRequestDto.BookingTypeId,
                            ClubId = id,
                            PaymentStatus = false,
                            UserId = LoginedAccount.UserId,
                        };

                        _service.BookingService.AddBooking(booking);

                        date = BookingRequestDto.BookDate;

                        for (int i = 1; i <= BookingRequestDto.WeekCount; i++)
                        {
                            BookingDetail bookingDetail = new()
                            {
                                BookingId = booking.BookingId,
                                BookDate = date,
                                CourtId = availableCourt.ElementAt(0).CourtId,
                                //SlotId = slotId,
                            };

                            _service.BookingDetailService.AddBookingDetail(bookingDetail);

                            date = date.AddDays(7);
                        }

                        TempData["Message"] = $"{MessagePrefix.SUCCESS}Đặt thành công";
                        return RedirectToPage("Book", new { id });
                    }
                    else
                    {
                        TempData["Message"] = $"{MessagePrefix.WARNING}Câu lạc bộ không còn sân trống";
                        return RedirectToPage("Book", new { id });
                    }
                }

                return RedirectToPage("/Index");
            }
            catch (Exception)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Lỗi hệ thống";
                return RedirectToPage("Book", new { id });
            }
        }
    }
}
