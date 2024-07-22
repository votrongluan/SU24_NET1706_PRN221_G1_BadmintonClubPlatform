using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using WebAppRazor.Constants;
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
            try
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
                    TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm thấy câu lạc bộ với id cần xóa";
                    return RedirectToPage("AllClubManage");
                }

                if (DeleteClub == null)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm thấy câu lạc bộ với id cần xóa";
                    return RedirectToPage("AllClubManage");
                }

                ClubDto = DeleteClub.ToResponseClubDto();

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        public IActionResult OnPost(int clubId)
        {
            try
            {
                try
                {
                    _service.ClubService.DeleteClub(clubId);
                    TempData["Message"] = $"{MessagePrefix.SUCCESS}Xóa câu lạc bộ với mã {clubId} thành công";
                }
                catch (Exception)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Xóa câu lạc bộ thất bại do lỗi hệ thống liên hệ đội ngũ để được hỗ trợ";
                }

                return RedirectToPage("AllClubManage");
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
