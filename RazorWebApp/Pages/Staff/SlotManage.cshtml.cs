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
            LoadAccountFromSession();

            Slots = _serviceManager.SlotService.GetAllSlot().Where(e => e.ClubId == LoginedAccount.ClubManageId).ToList();

            if (NewSlot.StartTime == null)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Thời gian bắt đầu không được để trống .";
                return RedirectToPage("/Staff/SlotManage");
            }

            if (NewSlot.EndTime == null)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Thời gian kết thúc không được để trống .";
                return RedirectToPage("/Staff/SlotManage");
            }

            if (NewSlot.Price == null) 
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Giá tiền không được để trống .";
                return RedirectToPage("/Staff/SlotManage");
            }

            if (NewSlot.StartTime >= NewSlot.EndTime) 
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Thời gian bắt đầu nhỏ hơn thời gian kết thúc .";
                return RedirectToPage("/Staff/SlotManage");
            }

            var club = _serviceManager.ClubService.GetClubById((int)LoginedAccount.ClubManageId);

            if (NewSlot.StartTime <= club.OpenTime) 
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Thời gian bắt đầu lớn hơn thời gian mở cửa.";
                return RedirectToPage("/Staff/SlotManage");
            }

            if (NewSlot.EndTime >= club.CloseTime)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR} Thời gian kết thúc phải nhỏ hơn thời gian đóng cửa.";
                return RedirectToPage("/Staff/SlotManage");
            }

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
