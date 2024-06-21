using System.Text.Json;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebAppRazor.Pages.Staff
{
    public class ClubManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _serviceManager;

        public ClubManageModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [BindProperty]
        public Club Club { get; set; }

        [BindProperty]
        public int CityId { get; set; }

        [BindProperty]
        public int DistrictId { get; set; }

        public SelectList Cities { get; set; }
        public SelectList Districts { get; set; }

        public string ConfirmationMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string accountJson = HttpContext.Session.GetString("Account");
            if (accountJson == null)
            {
                return RedirectToPage("/Authentication");
            }

            Account account = JsonSerializer.Deserialize<Account>(accountJson);
            int id = (int)account.ClubManageId;

            Club = _serviceManager.ClubService.GetClubById(id);

            if (Club == null)
            {
                return NotFound();
            }

            var cityService = _serviceManager.CityService.GetAllCities().ToList();
            Cities = new SelectList(cityService, "CityId", "CityName");
            CityId = Club.District.CityId; // Set the initial CityId
            var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
            Districts = new SelectList(districtService.Where(d => d.CityId == CityId), "DistrictId", "DistrictName");

            DistrictId = Club.DistrictId; // Set the initial DistrictId

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            string accountJson = HttpContext.Session.GetString("Account");
            if (accountJson == null)
            {
                return RedirectToPage("/Authentication");
            }

            Account account = JsonSerializer.Deserialize<Account>(accountJson);
            int id = (int)account.ClubManageId;

            // Update club properties with the values from the form
            Club.ClubId = id;

            try
            {
                _serviceManager.ClubService.UpdateClub(Club);
                ConfirmationMessage = "Câu lạc bộ đã được cập nhật thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(Club.ClubId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Staff/ClubManage");
        }



        public async Task<JsonResult> OnGetGetDistricts(int cityId)
        {
            var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
            var districts = districtService.Where(d => d.CityId == cityId).Select(d => new SelectListItem
            {
                Value = d.DistrictId.ToString(),
                Text = d.DistrictName
            });

            return new JsonResult(districts);
        }


        private bool ClubExists(int id)
        {
            return _serviceManager.ClubService.GetAllClubs().Any(e => e.ClubId == id);
        }
    }
}
