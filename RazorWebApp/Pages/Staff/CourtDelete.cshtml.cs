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
                return RedirectToPage("CourtManage", new { msg = "Kh�ng t�m th?y s�n v?i id c?n x�a" });
            }
            if (DeleteCourt == null)
            {
                return RedirectToPage("CourtManage", new { msg = "Kh�ng t�m th?y s�n v?i id c?n x�a" });
            }
            CourtDto = DeleteCourt.ToResponseCourtDto();

            return Page();
        }

        public IActionResult OnPost(int courtId)
        {
            try
            {
                _service.CourtService.DeleteCourt(courtId);
                return RedirectToPage("CourtManage", new { msg = $"X�a c�u l?c b? v?i m� {courtId} th�nh c�ng" });
            }
            catch (Exception ex)
            {
                return RedirectToPage("CourtManage", new { msg = $"X�a c�u l?c b? th?t b?i do l?i h? th?ng li�n h? ??i ng? ?? ???c h? tr?" });
            }
        }
    }
}
