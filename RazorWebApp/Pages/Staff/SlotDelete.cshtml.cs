using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff
{
    public class SlotDeleteModel : AuthorPageServiceModel
    {
        private readonly IServiceManager serviceManager;

        public SlotDeleteModel(IServiceManager _serviceManager)
        {
           serviceManager = _serviceManager;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Slot Slot { get; set; }

        public string TimeSlot { get; private set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Code go from here
            Slot = serviceManager.SlotService.GetSlotById(Id);

            if (Slot == null)
            {
                return NotFound();
            }

            // Format the time slot as StartTime - EndTime
            TimeSlot = $"{Slot.StartTime?.ToString("hh\\:mm")} - {Slot.EndTime?.ToString("hh\\:mm")}";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Update only the price for the slot
            var slotDelete = serviceManager.SlotService.GetSlotById(Id);

            if (slotDelete == null)
            {
                return NotFound();
            }

            serviceManager.SlotService.DeleteSlot(slotDelete.SlotId);

            TempData["Message"] = $"{MessagePrefix.SUCCESS}Khung giờ đã được xóa thành công.";

            return RedirectToPage("./SlotManage"); // Redirect to slot management page after update
        }
    }
}
