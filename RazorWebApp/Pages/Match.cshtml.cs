using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Services.IService;
using System.Globalization;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages
{
    public class MatchModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;  
        public List<Match> Matches { get; set; }
        public int TotalMatches { get; set; }
        public List<MatchResponseDto> MatchesDto { get; set; }
        public List<MatchResponseDto> FilterMatches { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Club> FilterClubs { get; set; }
        public List<City> Cities {  get; set; }
        public string Message { get; set; } = string.Empty;

        // Pagination properties
        public int currentPage { get; set; }
        public int totalPage { get; set; }

        public MatchModel(IServiceManager service)
        {
            _service = service;
        }

        public void InitializeData()
        {
            Matches = _service.MatchService.GetAllMatches();
            MatchesDto = Matches.Select(e => e.ToMatchResponseDto()).Where(x => x != null).ToList();
            Cities = _service.CityService.GetAllCities();
            TotalMatches = Matches.Count;
            Cities.Insert(0, new City
            {
                CityId = 0,
                CityName = "Mặc định",
            });
            FilterMatches = MatchesDto;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName");
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 1, int cityId = 0, int districtId = 0, DateOnly? matchDate = null)
        {
            const int pageSize = 5;

            FilterMatches = MatchesDto;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim().ToLower();
                FilterMatches = FilterMatches.Where(e =>
                    (searchProperty == "Title" && e.Title.ToLower().Contains(searchString)) ||
                    (searchProperty == "Description" && e.Description.ToLower().Contains(searchString)) ||
                    (searchProperty == "MatchDate" && e.MatchDate.Equals(matchDate)) ||
                    (searchProperty == "MatchTime" && e.MatchTime.ToLower().Contains(searchString)) ||
                    (searchProperty == "ClubName" && e.ClubName.ToLower().Contains(searchString)) ||
                    (searchProperty == "Address" && e.Address.ToLower().Contains(searchString))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterMatches = sortOrder switch
                {
                    -1 => sortProperty switch
                    {
                        "MatchId" => FilterMatches.OrderByDescending(x => x.MatchId).ToList(),
                        "Title" => FilterMatches.OrderByDescending(x => x.Title).ToList(),
                        "Description" => FilterMatches.OrderByDescending(x => x.Description).ToList(),
                        "MatchDate" => FilterMatches.OrderByDescending(x => x.MatchDate).ToList(),
                        "MatchTime" => FilterMatches.OrderByDescending(x => TimeSpan.Parse(x.MatchTime.Split('-')[0].Trim())).ToList(),
                        _ => FilterMatches
                    },
                    1 => sortProperty switch
                    {
                        "MatchId" => FilterMatches.OrderBy(x => x.MatchId).ToList(),
                        "Title" => FilterMatches.OrderBy(x => x.Title).ToList(),
                        "Description" => FilterMatches.OrderBy(x => x.Description).ToList(),
                        "MatchDate" => FilterMatches.OrderByDescending(x => x.MatchDate).ToList(),
                        "MatchTime" => FilterMatches.OrderBy(x => TimeSpan.Parse(x.MatchTime.Split('-')[0].Trim())).ToList(),
                        _ => FilterMatches
                    },
                    _ => FilterMatches
                };
            }

            if (districtId != 0)
            {
                var district = Cities.SelectMany(c => c.Districts).FirstOrDefault(d => d.DistrictId == districtId);
                if (district != null)
                {
                    FilterMatches = FilterMatches.Where(e => e.Address.Split(',').Any(part => part.Trim().Equals(district.DistrictName, StringComparison.OrdinalIgnoreCase))).ToList();

                }
            }
            else if (cityId != 0)
            {
                var city = Cities.FirstOrDefault(c => c.CityId == cityId);
                if (city != null)
                {
                    FilterMatches = FilterMatches.Where(e => e.Address.Contains(city.CityName)).ToList();
                }
            }
            if (matchDate != null)
            {
                FilterMatches = FilterMatches.Where(e => e.MatchDate.Equals(matchDate)).ToList();
            }

            TotalMatches = FilterMatches.Count;
            totalPage = (int)Math.Ceiling(TotalMatches / (double)pageSize);
            currentPage = Math.Max(1, Math.Min(page, totalPage));
            FilterMatches = FilterMatches.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder, int cityId, int districtId, int page = 1, DateOnly? matchDate = null)
        {
            if (TempData.ContainsKey("Message"))
            {
                Message = TempData["Message"].ToString();
            }

            InitializeData();
            Paging(searchString, searchProperty, sortProperty, sortOrder, page, cityId, districtId, matchDate);

            return Page();
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
