using System.Text.Json;
using Azure;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff
{
    public class SlotManageModel : AuthorPageServiceModel
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

        public async Task<IActionResult> OnGet(string sortProperty = "Hour", int sortOrder = 0)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

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

            int id = (int)LoginedAccount.ClubManageId;

            Slots = _serviceManager.SlotService.GetAllSlot().Where(e => e.ClubId == id).ToList();

            switch (sortProperty)
            {
                case "Hour":
                    if (sortOrder == 1)
                    {
                        Slots = Slots.OrderBy(s => s.StartTime).ToList();
                    }
                    else if (sortOrder == -1)
                    {
                        Slots = Slots.OrderByDescending(s => s.StartTime).ToList();
                    }
                    break;
                case "Price":
                    if (sortOrder == 1)
                    {
                        Slots = Slots.OrderBy(s => s.Price).ToList();
                    }
                    else if (sortOrder == -1)
                    {
                        Slots = Slots.OrderByDescending(s => s.Price).ToList();
                    }
                    break;
            }


            return Page();
        }

        public IActionResult OnPostAddSlot()
        {
            NewSlot.EndTime = NewSlot.StartTime.Value.AddMinutes(Duration);

            Slots = _serviceManager.SlotService.GetAllSlot();

            foreach (var slot in Slots)
            {
                if (NewSlot.StartTime >= slot.StartTime && NewSlot.StartTime < slot.EndTime)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Khung thời gian đã tồn tại.";
                    return RedirectToPage("/Staff/SlotManage");
                }
            }

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
