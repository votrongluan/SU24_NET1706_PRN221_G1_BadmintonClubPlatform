using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff;

public class SlotUpdateModel : AuthorPageServiceModel
{
    private readonly IServiceManager serviceManager;

    public SlotUpdateModel(IServiceManager _serviceManager)
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
        try
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            if (Id == null) return RedirectToPage("/NotFound");

            Slot = serviceManager.SlotService.GetSlotById(Id);

            if (Slot == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (Slot.ClubId != LoginedAccount.ClubManageId)
            {
                return RedirectToPage("/NotFound");
            }

            // Format the time slot as StartTime - EndTime
            TimeSlot = $"{Slot.StartTime?.ToString("hh\\:mm")} - {Slot.EndTime?.ToString("hh\\:mm")}";

            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            // Update only the price for the slot
            var slotToUpdate = serviceManager.SlotService.GetSlotById(Id);

            if (slotToUpdate == null)
            {
                return RedirectToPage("/NotFound");
            }

            slotToUpdate.Price = Slot.Price;

            serviceManager.SlotService.UpdateSlot(slotToUpdate);

            TempData["Message"] = $"{MessagePrefix.SUCCESS}Khung giờ đã được cập nhật thành công.";

            return RedirectToPage("./SlotManage"); // Redirect to slot management page after update
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }
}