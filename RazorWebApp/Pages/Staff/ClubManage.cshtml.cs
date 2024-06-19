using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using System.Text.Json;

namespace RazorWebApp.Pages.Staff
{
    public class ClubManageModel : PageModel
    {
        private readonly BusinessObjects.Entities.BcbpContext _context;
        private readonly IServiceManager _serviceManager;

        public ClubManageModel(BusinessObjects.Entities.BcbpContext context, IServiceManager serviceManager)
        {
            _context = context;
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
            CityId = Club.District.CityId;
            var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
            Districts = new SelectList(districtService.Where(d => d.CityId == CityId), "DistrictId", "DistrictName");

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

            var existingClub = _serviceManager.ClubService.GetClubById(id);

            if (existingClub == null)
            {
                return NotFound();
            }

            // Cập nhật thuộc tính của câu lạc bộ hiện có với các giá trị mới
            existingClub.ClubName = Club.ClubName;
            existingClub.Address = Club.Address;
            existingClub.ClubEmail = Club.ClubEmail;
            existingClub.ClubPhone = Club.ClubPhone;
            existingClub.FanpageLink = Club.FanpageLink;
            existingClub.AvatarLink = Club.AvatarLink;
            existingClub.OpenTime = Club.OpenTime;
            existingClub.CloseTime = Club.CloseTime;
            existingClub.DistrictId = DistrictId;


            try
            {
                _serviceManager.ClubService.UpdateClub(existingClub);
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

            return Page();
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
