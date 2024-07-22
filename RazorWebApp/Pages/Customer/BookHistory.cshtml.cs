using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Customer
{
    public class BookHistoryModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<BookHistoryModel> _logger;
        public BookHistoryModel(IServiceManager serviceManager, ILogger<BookHistoryModel> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        public List<Booking> Bookings { get; set; }
        public List<Account> Accounts { get; set; }
        public int TotalBookingHistory { get; set; }
        public List<Court> Courts { get; set; }
        public List<BookingHistoryResponseDto> bookingHistoryResponsesDto { get; set; }
        public List<BookingHistoryResponseDto> FilterBookings { get; set; }
        public string Message { get; set; } = string.Empty;
        public int currentPage { get; set; }
        public int totalPages { get; set; }

        public void InitializeData()
        {
            Courts = _serviceManager.CourtService.GetAllCourts();
            Bookings = _serviceManager.BookingService.GetAllBookings().Where(e => e.UserId == LoginedAccount.UserId).ToList();
            Accounts = _serviceManager.AccountService.GetAllAccount();
            TotalBookingHistory = Bookings.Count;
            bookingHistoryResponsesDto = Bookings.Select(e => e.ToBookingHistory()).ToList();
            FilterBookings = bookingHistoryResponsesDto;
        }
        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder, DateOnly? bookingDate)
        {
            try
            {
                LoadAccountFromSession();
                var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());
                int page = Convert.ToInt32(Request.Query["page"]);

                if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

                _logger.LogInformation("OnGet called with: searchString={SearchString}, searchProperty={SearchProperty}, sortProperty={SortProperty}, sortOrder={SortOrder}, page={Page}, bookingDate={BookingDate}",
                    searchString, searchProperty, sortProperty, sortOrder, page, bookingDate);

                if (!string.IsNullOrWhiteSpace(Message))
                {
                    Message = string.Empty;
                }

                if (TempData.ContainsKey("Message"))
                {
                    Message = TempData["Message"].ToString();
                }

                InitializeData();
                Paging(searchString, searchProperty, sortProperty, sortOrder, page, bookingDate: bookingDate);

                _logger.LogInformation("After Paging: FilterBookings count = {FilterBookingsCount}, currentPage={CurrentPage}, totalPage={TotalPage}",
                    FilterBookings.Count, currentPage, totalPages);

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0, DateOnly? bookingDate = null)
        {
            _logger.LogInformation("Paging called with: searchString={SearchString}, searchProperty={SearchProperty}, sortProperty={SortProperty}, sortOrder={SortOrder}, page={Page}, bookingDate={BookingDate}",
                searchString, searchProperty, sortProperty, sortOrder, page, bookingDate);

            const int pageSize = 5;

            FilterBookings = bookingHistoryResponsesDto;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterBookings = searchProperty switch
                {
                    "BookingId" => FilterBookings.Where(x => x.BookingId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubName" => FilterBookings.Where(x => x.ClubName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "DistrictName" => FilterBookings.Where(x => x.DistrictName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "CityName" => FilterBookings.Where(x => x.CityName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "BookingDate" => FilterBookings.Where(x => x.BookingDate.Equals(searchString)).ToList(),
                    "BookingTime" => FilterBookings.Where(x => x.BookingTime.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => FilterBookings
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterBookings = (sortProperty, sortOrder) switch
                {
                    ("BookingId", -1) => FilterBookings.OrderByDescending(x => x.BookingId).ToList(),
                    ("BookingId", 1) => FilterBookings.OrderBy(x => x.BookingId).ToList(),
                    ("ClubName", -1) => FilterBookings.OrderByDescending(x => x.ClubName).ToList(),
                    ("ClubName", 1) => FilterBookings.OrderBy(x => x.ClubName).ToList(),
                    ("DistrictName", -1) => FilterBookings.OrderByDescending(x => x.DistrictName).ToList(),
                    ("DistrictName", 1) => FilterBookings.OrderBy(x => x.DistrictName).ToList(),
                    ("CityName", -1) => FilterBookings.OrderByDescending(x => x.CityName).ToList(),
                    ("CityName", 1) => FilterBookings.OrderBy(x => x.CityName).ToList(),
                    ("BookingDate", -1) => FilterBookings.OrderByDescending(x => x.BookingDate).ToList(),
                    ("BookingDate", 1) => FilterBookings.OrderBy(x => x.BookingDate).ToList(),
                    ("BookingTime", -1) => FilterBookings.OrderByDescending(x => x.BookingTime).ToList(),
                    ("BookingTime", 1) => FilterBookings.OrderBy(x => x.BookingTime).ToList(),
                    _ => FilterBookings
                };
            }

            if (bookingDate.HasValue)
            {
                _logger.LogInformation("Filtering by BookingDate: {BookingDate}", bookingDate.Value);
                FilterBookings = FilterBookings.Where(e => e.BookingDate == bookingDate.Value.ToString("dd/MM/yyyy")).ToList();
            }

            page = page == 0 ? 1 : page;
            TotalBookingHistory = FilterBookings.Count;
            totalPages = (int)Math.Ceiling(FilterBookings.Count / (double)pageSize);
            currentPage = page;
            FilterBookings = FilterBookings.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            _logger.LogInformation("After paging: FilterBookings count = {FilterBookingsCount}, currentPage={CurrentPage}, totalPage={TotalPage}",
                FilterBookings.Count, currentPage, totalPages);
        }
    }
}
