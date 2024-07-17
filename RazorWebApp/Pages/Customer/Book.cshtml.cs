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
        public Club BookClub { get; set; }
        public ResponseClubDto Club { get; set; }
        public List<CourtType> CourtTypes { get; set; }
        public List<Slot> Slots { get; set; }
        public List<BookingType> BookingTypes { get; set; }
        public int MinHour { get; set; }
        public int MaxHour { get; set; }

        public void InitializeData()
        {
            BookClub = _service.ClubService.GetClubById(ClubId);
            MinHour = BookClub.OpenTime.Value.Hour + (BookClub.OpenTime.Value.Minute > 0 ? 1 : 0);
            MaxHour = BookClub.CloseTime.Value.Hour - 1;
            Club = BookClub.ToResponseClubDto();
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

            if (BookClub.Status == false) return RedirectToPage("/NotFound");

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

                BookingRequestDto.ClubId = id;
                BookingRequestDto.UserId = LoginedAccount.UserId;
                BookingRequestDto.StartTime =
                    new TimeOnly(BookingRequestDto.StartTimeHour, BookingRequestDto.StartTimeMinute);
                BookingRequestDto.EndTime = BookingRequestDto.StartTime.AddHours(BookingRequestDto.Duration);
                BookingRequestDto.DefaultPrice = (int)bookClub.DefaultPricePerHour;

                if (BookingRequestDto.BookingTypeId == (int)BookingTypeEnum.LichNgay)
                {
                    BookingRequestDto.WeekCount = 1;
                }

                var date = BookingRequestDto.BookDate;

                for (int i = 1; i <= BookingRequestDto.WeekCount; i++)
                {
                    var bookingDetail = _service.BookingDetailService.GetAllBookingDetails()
                        .Where(e => e.BookDate == date && e.Booking.UserId == LoginedAccount.UserId).OrderBy(e => e.StartTime).ToList();

                    foreach (var detail in bookingDetail)
                    {
                        if (BookingRequestDto.StartTime < detail.EndTime && detail.EndTime < BookingRequestDto.EndTime)
                        {
                            TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại, bạn đã đặt lịch cho ngày hôm đó với giờ trùng nhau";
                            return RedirectToPage("Book", new { id });
                        }
                    }

                    date = date.AddDays(7);
                }

                if (BookingRequestDto.StartTime < bookClub.OpenTime || BookingRequestDto.EndTime > bookClub.CloseTime || BookingRequestDto.EndTime < bookClub.OpenTime)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Đặt thất bại khung giờ bạn chọn club chưa mở cửa";
                    return RedirectToPage("Book", new { id });
                }

                if (bookClub.DefaultPricePerHour == 0 || bookClub.DefaultPricePerHour == null)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Câu lạc bộ này chưa đăng ký giá";
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
