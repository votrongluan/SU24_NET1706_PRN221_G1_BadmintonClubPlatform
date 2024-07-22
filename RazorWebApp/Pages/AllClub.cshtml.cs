using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;

namespace WebAppRazor.Pages
{
    public class AllClubModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        public int FindCount { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Club> FilterClubs { get; set; }
        public List<City> Cities { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public AllClubModel(IServiceManager serivce)
        {
            _service = serivce;
        }

        private void InitializeData()
        {
            Clubs = _service.ClubService.GetAllClubs();
            FindCount = Clubs.Count;
            Cities = _service.CityService.GetAllCities();
            Cities.Insert(0, new City()
            {
                CityId = 0,
                CityName = "Mặc định",
            });

            FilterClubs = Clubs;

            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName");
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0, int cityId = 0, int districtId = 0)
        {
            const int PageSize = 16;  // Set the number of items per page

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterClubs = searchProperty switch
                {
                    "ClubId" => Clubs.Where(e => e.ClubId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubName" => Clubs.Where(e => e.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "Address" => Clubs.Where(e => e.Address.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubPhone" => Clubs.Where(e => e.ClubPhone.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => FilterClubs,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterClubs = sortProperty switch
                {
                    "Star" => sortOrder == -1 ? FilterClubs.OrderByDescending(e => e.TotalReview != 0 ? e.TotalStar / e.TotalReview * 1.0 : -1).ToList() : sortOrder == 1 ? FilterClubs.OrderBy(e => e.TotalReview != 0 ? e.TotalStar / e.TotalReview * 1.0 : -1).ToList() : FilterClubs,
                    "Book" => sortOrder == -1 ? FilterClubs.OrderByDescending(e => e.Bookings.Count).ToList() : sortOrder == 1 ? FilterClubs.OrderBy(e => e.Bookings.Count).ToList() : FilterClubs,
                    "Review" => sortOrder == -1 ? FilterClubs.OrderByDescending(e => e.TotalReview).ToList() : sortOrder == 1 ? FilterClubs.OrderBy(e => e.TotalReview).ToList() : FilterClubs,
                    _ => FilterClubs,
                };
            }

            if (districtId != 0)
            {
                FilterClubs = FilterClubs.Where(e => e.DistrictId == districtId).ToList();
            }
            else if (cityId != 0)
            {
                FilterClubs = FilterClubs.Where(e => e.District.CityId == cityId).ToList();
            }

            // Pagination logic
            page = page == 0 ? 1 : page;
            FindCount = FilterClubs.Count;
            TotalPages = (int)Math.Ceiling(FilterClubs.Count / (double)PageSize);
            CurrentPage = page;
            FilterClubs = FilterClubs.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet(int cityId, int districtId, string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            try
            {
                LoadAccountFromSession();

                if (LoginedAccount != null)
                {
                    var role = (string)LoginedAccount.Role;
                    if (role == AccountRoleEnum.Admin.ToString()) return RedirectToPage("/Admin/Index");
                    if (role == AccountRoleEnum.Staff.ToString()) return RedirectToPage("/Staff/Index");
                }

                InitializeData();

                int page = Convert.ToInt32(Request.Query["page"]);
                Paging(searchString, searchProperty, sortProperty, sortOrder, page, cityId: cityId, districtId: districtId);

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        public JsonResult OnGetDistrictsByCityId(int cityId)
        {
            InitializeData();
            var districts = _service.DistrictService.GetAllDistrictsByCityId(cityId);
            districts.Insert(0, new District()
            {
                DistrictId = 0,
                DistrictName = "Mặc định",
            });
            return new JsonResult(districts);
        }
    }
}
