using System.Text.Json;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

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
        public string ErrorMessage { get; set; }


        public async Task<IActionResult> OnGet()
        {
            Slots =_serviceManager.SlotService.GetAllSlot();
            return Page();
        }

        public IActionResult OnPostAddSlot()
        {
            // Validate the new slot's start time
            foreach (var slot in Slots)
            {
                if (NewSlot.StartTime >= slot.StartTime && NewSlot.StartTime < slot.EndTime)
                {
                    ErrorMessage = "Giờ bắt đầu nằm trong khoảng khung giờ đã có sẵn.";
                    return Page();
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

            //NewSlot.EndTime = NewSlot.StartTime.AddMinutes(NewSlot.Duration);
            
            NewSlot.ClubId = id;

            _serviceManager.SlotService.AddSlot(NewSlot);

            return RedirectToPage("/Staff/SlotManage");
        }
    }
}
