using BusinessObjects.Dtos.Court;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff;

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
        try
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Validate club id is active
            //-------------------------------
            if (LoginedAccount.ClubManageId == null)
            {
                return RedirectToPage("/NotFound");
            }

            int validateClubId = (int)LoginedAccount.ClubManageId;

            var isActiveClubById = _service.ClubService.GetDeActiveClubById(validateClubId);

            if (isActiveClubById != null)
            {
                return RedirectToPage("/NotFound");
            }
            //-------------------------------
            // End of validate club is active

            if (id != null)
            {
                DeleteCourt = _service.CourtService.GetCourtById(id ?? -1);
            }
            else
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm thấy sân với id cần xóa";
                return RedirectToPage("CourtManage");
            }

            if (DeleteCourt == null)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm sân với id cần xóa";
                return RedirectToPage("CourtManage");
            }

            if (DeleteCourt.ClubId != LoginedAccount.ClubManageId)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm sân với id cần xóa";
                return RedirectToPage("CourtManage");
            }

            CourtDto = DeleteCourt.ToResponseCourtDto();

            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Error");
        }
    }

    public IActionResult OnPost(int courtId)
    {
        try
        {
            _service.CourtService.DeleteCourt(courtId);

            TempData["Message"] = $"{MessagePrefix.SUCCESS}Xóa câu lạc bộ với mã {courtId} thành công";
            return RedirectToPage("CourtManage");
        }
        catch (Exception)
        {
            TempData["Message"] = $"{MessagePrefix.ERROR}Xóa câu lạc bộ thất bại vui lòng liên hệ đội ngũ để được hỗ trợ";
            return RedirectToPage("CourtManage");
        }
    }
}