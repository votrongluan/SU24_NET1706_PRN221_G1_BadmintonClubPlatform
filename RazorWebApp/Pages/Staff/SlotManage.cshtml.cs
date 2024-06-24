using System.Text.Json;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff
{
    public class SlotManageModel : PageModel
    {
        private readonly IServiceManager _serviceManager;

        public SlotManageModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [BindProperty]
        public List<Slot> Slots { get; set; }

        [BindProperty]
        public Slot NewSlot { get; set; }

        [BindProperty]
        public int Duration { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Set and clear the message
            if (!string.IsNullOrWhiteSpace(Message))
            {
                Message = string.Empty;
            }

            if (TempData.ContainsKey("Message"))
            {
                Message = TempData["Message"].ToString();
            }

            Slots = _serviceManager.SlotService.GetAllSlot();
            return Page();
        }

        public IActionResult OnPostAddSlot()
        {
            // Calculate EndTime based on StartTime and Duration
            NewSlot.EndTime = NewSlot.StartTime.Value.AddMinutes(Duration);

            Slots = _serviceManager.SlotService.GetAllSlot();

            // Validate the new slot's start time
            foreach (var slot in Slots)
            {
                if (NewSlot.StartTime >= slot.StartTime && NewSlot.StartTime < slot.EndTime)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Khung thời gian đã tồn tại.";
                    return RedirectToPage("/Staff/SlotManage");
                }
            }

            // If valid, add the new slot (this is just a mock, you would save to the database)
            string accountJson = HttpContext.Session.GetString("Account");
            if (accountJson == null)
            {
                return RedirectToPage("/Authentication");
            }

            Account account = JsonSerializer.Deserialize<Account>(accountJson);
            int id = (int)account.ClubManageId;

            NewSlot.ClubId = id;
            _serviceManager.SlotService.AddSlot(NewSlot);

            TempData["Message"] = $"{MessagePrefix.SUCCESS}Câu lạc bộ đã được cập nhật thành công.";
            return RedirectToPage("/Staff/SlotManage");
        }
    }
}
