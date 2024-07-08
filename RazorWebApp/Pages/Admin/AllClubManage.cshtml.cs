using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class AllClubManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public CreateClubDto CreatedClub { get; set; }
        public List<City> Cities { get; set; }
        public List<Club> Clubs { get; set; }
        public List<ResponseClubDto> ClubsDto { get; set; }
        public List<ResponseClubDto> FilterClubsDto { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // MESSAGE FOR ACTION
        public string Message { get; set; }
        public int TotalFindClub { get; set; } = 0;

        private void InitializeData()
        {
            Clubs = _service.ClubService.GetAllClubs();
            Cities = _service.CityService.GetAllCities();

            ClubsDto = Clubs.Select(e => e.ToResponseClubDto()).ToList();
            FilterClubsDto = ClubsDto;

            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName");
        }

        public AllClubManageModel(IServiceManager service)
        {
            _service = service;
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0)
        {
            const int PageSize = 10;  // Set the number of items per page

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterClubsDto = searchProperty switch
                {
                    "ClubId" => ClubsDto.Where(e => e.ClubId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubName" => ClubsDto.Where(e => e.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "Address" => ClubsDto.Where(e => e.Address.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubPhone" => ClubsDto.Where(e => e.ClubPhone.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => FilterClubsDto,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterClubsDto = sortProperty switch
                {
                    "ClubId" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubId).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubId).ToList() : FilterClubsDto,
                    "ClubName" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubName).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubName).ToList() : FilterClubsDto,
                    "Address" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.Address).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.Address).ToList() : FilterClubsDto,
                    "ClubPhone" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubPhone).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubPhone).ToList() : FilterClubsDto,
                    _ => FilterClubsDto,
                };
            }

            TotalFindClub = FilterClubsDto?.Count ?? 0;

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(FilterClubsDto.Count / (double)PageSize);
            FilterClubsDto = FilterClubsDto.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

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

            InitializeData();

            int page = Convert.ToInt32(Request.Query["page"]);
            Paging(searchString, searchProperty, sortProperty, sortOrder, page);

            return Page();
        }

        public IActionResult OnPost()
        {
            Message = string.Empty;

            if (!ModelState.IsValid)
            {
                // Build the error message from ModelState
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var combinedErrorMessage = string.Join("\n", errorMessages);

                // Set the message with the error prefix and combined error messages
                TempData["Message"] = $"{MessagePrefix.ERROR} Dữ liệu bạn nhập có lỗi:\n{combinedErrorMessage}";
                return RedirectToPage("AllClubManage");
            }

            try
            {
                var result = _service.ClubService.CheckPhoneExisted(CreatedClub.ClubPhone);
                if (result == false)
                {
                    _service.ClubService.AddClub(CreatedClub.ToClub());

                    TempData["Message"] = $"{MessagePrefix.SUCCESS}Tạo mới câu lạc bộ thành công";
                    return RedirectToPage("AllClubManage");
                }
                else
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Số điện thoại đã tồn tại trong hệ thống";
                    return RedirectToPage("AllClubManage");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Câu lạc bộ không được tạo do lỗi hệ thống vui lòng liên hệ đội ngũ hỗ trợ";
                return RedirectToPage("AllClubManage");
            }
        }

        public JsonResult OnGetDistrictsByCityId(int cityId)
        {
            InitializeData();
            var districts = _service.DistrictService.GetAllDistrictsByCityId(cityId);
            return new JsonResult(districts);
        }
    }
}