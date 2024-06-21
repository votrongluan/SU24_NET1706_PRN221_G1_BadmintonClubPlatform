using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class ClubDeleteModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;

        public Club DeleteClub { get; set; }
        public ResponseClubDto ClubDto { get; set; }

        public ClubDeleteModel(IServiceManager service)
        {
            _service = service;
        }

        public IActionResult OnGet(int? id)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            if (id != null)
            {
                DeleteClub = _service.ClubService.GetClubById(id ?? -1);
            }
            else
            {
                return RedirectToPage("AllClubManage", new { ErrorMessage = "Không tìm thấy câu lạc bộ với id cần xóa" });
            }

            if (DeleteClub == null)
            {
                return RedirectToPage("AllClubManage", new { ErrorMessage = "Không tìm thấy câu lạc bộ với id cần xóa" });
            }

            ClubDto = DeleteClub.ToResponseClubDto();

            return Page();
        }

        public IActionResult OnPost(int clubId)
        {
            try
            {
                _service.ClubService.DeleteClub(clubId);
                return RedirectToPage("AllClubManage", new { SuccessMessage = $"Xóa câu lạc bộ với mã {clubId} thành công" });
            }
            catch (Exception ex)
            {
                return RedirectToPage("AllClubManage", new { ErrorMessage = $"Xóa câu lạc bộ thất bại do lỗi hệ thống liên hệ đội ngũ để được hỗ trợ" });
            }
        }
    }
}
