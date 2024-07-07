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
            BookingTypes = _service.AvailableBookingTypeService.GetAvailableBookingTypesByClubId(ClubId)
                .Select(e => e.BookingType).Where(e => e.BookingTypeId != (int)BookingTypeEnum.LichThiDau).ToList();
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("/NotFound");

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

        public IActionResult OnPost(int id)
        {
            LoadAccountFromSession();

            if (BookingRequestDto.BookDate < DateOnly.FromDateTime(DateTime.Now.AddDays(3)))
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Ngày chưa chính xác, phải lớn hơn hôm nay 3 ngày";
                return RedirectToPage("Book", new { id });
            }

            try
            {
                var bookClub = _service.ClubService.GetClubById(id);
                var bookingDetail = _service.BookingDetailService.GetAllBookingDetails()
                    .Where(e => e.BookDate == BookingRequestDto.BookDate);

                if (bookingDetail.Count() > 0)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại, bạn đã đặt lịch cho ngày hôm đó";
                    return RedirectToPage("Book", new { id });
                }

                BookingRequestDto.ClubId = id;
                BookingRequestDto.UserId = LoginedAccount.UserId;
                BookingRequestDto.StartTime =
                    new TimeOnly(BookingRequestDto.StartTimeHour, BookingRequestDto.StartTimeMinute);
                BookingRequestDto.EndTime = BookingRequestDto.StartTime.AddHours(BookingRequestDto.Duration);
                BookingRequestDto.DefaultPrice = (int)bookClub.DefaultPricePerHour;

                if (BookingRequestDto.StartTime < bookClub.OpenTime || BookingRequestDto.EndTime > bookClub.CloseTime)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại khung giờ bạn chọn club chưa mở cửa";
                    return RedirectToPage("Book", new { id });
                }

                if (BookingRequestDto.BookingTypeId == (int)BookingTypeEnum.LichNgay)
                {
                    var result = _service.BookingService.BookLichNgay(BookingRequestDto);

                    if (result.status)
                    {
                        return RedirectToPage("/Customer/BookDetail", new { bookId = result.bookId });
                    }

                    TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại";
                    return RedirectToPage("Book", new { id });
                }

                if (BookingRequestDto.BookingTypeId == (int)BookingTypeEnum.LichCoDinh)
                {
                    var result = _service.BookingService.BookLichCoDinh(BookingRequestDto);

                    if (result.status)
                    {
                        return RedirectToPage("/Customer/BookDetail", new { bookId = result.bookId });
                    }

                    TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại";
                    return RedirectToPage("Book", new { id });
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
