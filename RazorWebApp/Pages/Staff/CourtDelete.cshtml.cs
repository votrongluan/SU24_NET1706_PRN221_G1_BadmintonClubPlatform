using BusinessObjects.Dtos.Court;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class CourtDeleteModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        public Court DeleteCourt { get; set; }
        public ResponseCourtDto CourtDto { get; set; }
        public List<CourtType> CourtTypes { get; set; }
        public List<Club> Clubs { get; set; }
        public CourtDeleteModel(IServiceManager service)
        {
            _service = service;
        }

        public IActionResult OnGet(int? id)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            if (id != null)
            {
                DeleteCourt = _service.CourtService.GetCourtById(id ?? -1);
            }
            else
            {
                return RedirectToPage("CourtManage", new { msg = "Không tìm th?y sân v?i id c?n xóa" });
            }
            if (DeleteCourt == null)
            {
                return RedirectToPage("CourtManage", new { msg = "Không tìm th?y sân v?i id c?n xóa" });
            }
            CourtDto = DeleteCourt.ToResponseCourtDto();

            return Page();
        }

        public IActionResult OnPost(int courtId)
        {
            try
            {
                _service.CourtService.DeleteCourt(courtId);
                return RedirectToPage("CourtManage", new { msg = $"Xóa câu l?c b? v?i mã {courtId} thành công" });
            }
            catch (Exception ex)
            {
                return RedirectToPage("CourtManage", new { msg = $"Xóa câu l?c b? th?t b?i do l?i h? th?ng liên h? ??i ng? ?? ???c h? tr?" });
            }
        }
    }
}
