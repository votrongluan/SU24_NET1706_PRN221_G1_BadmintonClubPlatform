using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Net.payOS;
using Net.payOS.Types;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Customer
{
    public class BookDetailModel : AuthorPageServiceModel
    {
        private IServiceManager _service;
        public Booking Booking { get; set; }
        public Club Club { get; set; }
        public string Message { get; set; }

        private void InitializeData(int id)
        {
            Booking = _service.BookingService.GetBookingById(id);
            Club = _service.ClubService.GetClubById(Booking?.ClubId ?? -1);
        }

        public BookDetailModel(IServiceManager service)
        {
            _service = service;
        }

        public IActionResult OnGet(int? bookId)
        {
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

            // Validate route id
            if (bookId == null) return NotFound();

            InitializeData((int)bookId);

            if (Booking == null) return NotFound();

            if (Booking.UserId != LoginedAccount.UserId) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnGetPay(int? bookId)
        {
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

            // Validate route id
            if (bookId == null) return NotFound();

            InitializeData((int)bookId);

            if (Booking == null) return NotFound();

            if (Booking.UserId != LoginedAccount.UserId) return NotFound();

            if (Booking.PaymentStatus == true) return RedirectToPage("BookDetail", new { bookId = Booking.BookingId });

            try
            {
                PayOS payOs = new PayOS(Club.ClientId, Club.ApiKey, Club.ChecksumKey);

                PaymentLinkInformation paymentLinkInformation =
                    await payOs.getPaymentLinkInformation(Booking.BookingId);

                var dateArray = Booking.BookingDetails.Select(e => e.BookDate.Value.ToString("dd/MM/yyyy"));
                var dateString = string.Join(" , ", dateArray);

                var itemDataName = $"Thanh toan ma don dat san {Booking.BookingId}";
                var itemDataQuantity = Booking.BookingDetails.Count;
                var itemDataPrice = Booking?.TotalPrice ?? 0;

                List<ItemData> items = new()
                {
                    new(itemDataName,itemDataQuantity,itemDataPrice),
                };

                long orderId = ReadNextOrderId();

                PaymentData paymentData = new PaymentData(orderId, Booking?.TotalPrice ?? 0,
                    $"Thanh toan don dat san", items, $"http://localhost:5072/Customer/BookDetail?bookId={Booking.BookingId}&handler=PayFail", $"http://localhost:5072/Customer/BookDetail?bookId={Booking.BookingId}&handler=PaySuccess&orderId={orderId}");

                CreatePaymentResult createPayment = await payOs.createPaymentLink(paymentData);

                WriteNextOrderId(orderId + 1);

                return Redirect(createPayment.checkoutUrl);
            }
            catch (Exception)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Có lỗi từ hệ thống trong quá trình thanh toán";
                return RedirectToPage("BookDetail", new { bookId = Booking.BookingId });
            }
        }

        public IActionResult OnGetPaySuccess(int? bookId)
        {
            // Validate route id
            if (bookId == null) return NotFound();

            InitializeData((int)bookId);

            if (Booking == null) return NotFound();

            Booking.PaymentStatus = true;
            _service.BookingService.UpdateBooking(Booking);

            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            if (Booking.UserId != LoginedAccount.UserId) return NotFound();

            TempData["Message"] = $"{MessagePrefix.SUCCESS} Đơn đặt sân thanh toán thành công";
            return RedirectToPage("BookDetail", new { bookId = Booking.BookingId });
        }

        public IActionResult OnGetPayFail(int? bookId, long orderId)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Customer.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Validate route id
            if (bookId == null) return NotFound();

            InitializeData((int)bookId);

            if (Booking == null) return NotFound();

            if (Booking.UserId != LoginedAccount.UserId) return NotFound();

            try
            {
                PayOS payOs = new PayOS(Club.ClientId, Club.ApiKey, Club.ChecksumKey);

                payOs.cancelPaymentLink(orderId);
            }
            catch (Exception)
            {
                // ignored
            }

            TempData["Message"] = $"{MessagePrefix.ERROR} Đơn đặt sân thanh toán không thành công";
            return RedirectToPage("BookDetail", new { bookId = Booking.BookingId });
        }

        private long ReadNextOrderId()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/nextOrderId.txt");

            if (!System.IO.File.Exists(filePath))
            {
                // If the file does not exist, initialize it with a starting value
                System.IO.File.WriteAllText(filePath, "100");
            }

            var nextOrderIdStr = System.IO.File.ReadAllText(filePath);
            long nextOrderId = long.Parse(nextOrderIdStr);

            return nextOrderId;
        }

        private void WriteNextOrderId(long nextOrderId)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/nextOrderId.txt");
            System.IO.File.WriteAllText(filePath, nextOrderId.ToString());
        }
    }
}
