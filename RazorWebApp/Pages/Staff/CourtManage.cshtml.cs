using BusinessObjects.Dtos.Court;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class CourtManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public CreateCourtDto CreateCourt { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Court> Courts { get; set; }
        public List<Club> Clubs { get; set; }
        public List<CourtType> CourtTypes { get; set; }
        public List<ResponseCourtDto> CourtsDto { get; set; }
        public List<ResponseCourtDto> FilterCourtsDto { get; set; }
        private void InitializeData()
        {
            CourtTypes = _service.CourtTypeService.GetAllCourtTypes();
            Clubs = _service.ClubService.GetAllClubs();

            int clubManageId = LoginedAccount?.ClubManageId ?? 0;
            Courts = _service.CourtService.GetCourtsByClubId(clubManageId);

            CourtsDto = Courts.Select(e => e.ToResponseCourtDto()).ToList();
            FilterCourtsDto = CourtsDto;

            ViewData["CourtTypeId"] = new SelectList(CourtTypes, "CourtTypeId", "TypeName");
        }
        public CourtManageModel(IServiceManager service)
        {
            _service = service;
        }
        private void Filter(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterCourtsDto = searchProperty switch
                {
                    "CourtId" => CourtsDto.Where(e => e.CourtId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "TypeName" => CourtsDto.Where(e => CourtTypes.FirstOrDefault(ct => ct.CourtTypeId == e.CourtTypeId)?.TypeName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
                    _ => FilterCourtsDto,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterCourtsDto = sortProperty switch
                {
                    "CourtId" => sortOrder == -1 ? FilterCourtsDto.OrderByDescending(e => e.CourtId).ToList() : sortOrder == 1 ? FilterCourtsDto.OrderBy(e => e.CourtId).ToList() : FilterCourtsDto,
                    "TypeName" => sortOrder == -1 ? FilterCourtsDto.OrderByDescending(e => e.CourtTypeId).ThenBy(e => e.CourtId).ToList() :
                         sortOrder == 1 ? FilterCourtsDto.OrderBy(e => e.CourtTypeId).ThenBy(e => e.CourtId).ToList() :
                         FilterCourtsDto.OrderBy(e => e.CourtTypeId == 1 ? 0 : e.CourtTypeId == 2 ? 1 : 2).ThenBy(e => e.CourtId).ToList(),
                    _ => FilterCourtsDto,
                };
            }
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            string msg = Request.Query["msg"];

            if (!string.IsNullOrEmpty(msg))
            {
                Message = msg;
            }
            else
            {
                Message = string.Empty;
            }
            InitializeData();

            Filter(searchString, searchProperty, sortProperty, sortOrder);

            return Page();
        }

        public IActionResult OnPost()
        {
            Message = string.Empty;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                LoadAccountFromSession();
                var accLog = LoginedAccount.ClubManageId;
                var clubId = _service.ClubService.GetAllClubs();

                var court = new Court
                {
                    CourtTypeId = CreateCourt.CourtTypeId,
                    ClubId = CreateCourt.ClubId
                };

                _service.CourtService.AddCourt(court);
                InitializeData();
                Filter("", "", "", 0);
                Message = "T?o m?i sân thành công";
            }
            catch (Exception ex)
            {
                InitializeData();
                Filter("", "", "", 0);
                Message = "Sân không ???c t?o do l?i h? th?ng vui lòng liên h? ??i ng? h? tr?";
            }
            return Page();
        }

        public JsonResult OnGetCourtsByClubId(int id)
        {
            InitializeData();
            var courts = _service.CourtService.GetCourtsByClubId(id);
            return new JsonResult(courts);
        }
    }
}
