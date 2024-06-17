using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessObjects;

namespace RazorWebApp.Pages.Staff
{
    public class ClubManageModel : PageModel
    {
        private readonly BusinessObjects.Entities.BcbpContext _context;

        public ClubManageModel(BusinessObjects.Entities.BcbpContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Club Club { get; set; }

        [BindProperty]
        public int CityId { get; set; }

        [BindProperty]
        public int DistrictId { get; set; }

        public SelectList Cities { get; set; }
        public SelectList Districts { get; set; }
        public SelectList Clubs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Cities = new SelectList(await _context.Cities.ToListAsync(), "CityId", "CityName");
            Clubs = new SelectList(await _context.Clubs.ToListAsync(), "ClubId", "ClubName");

            if (id == null)
            {
                return Page();
            }

            Club = await _context.Clubs.FindAsync(id);

            if (Club == null)
            {
                return NotFound();
            }

            CityId = Club.District.CityId;
            Districts = new SelectList(await _context.Districts.Where(d => d.CityId == CityId).ToListAsync(), "DistrictId", "DistrictName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Cities = new SelectList(await _context.Cities.ToListAsync(), "CityId", "CityName");
                Clubs = new SelectList(await _context.Clubs.ToListAsync(), "ClubId", "ClubName");
                Districts = new SelectList(await _context.Districts.Where(d => d.CityId == CityId).ToListAsync(), "DistrictId", "DistrictName");
                return Page();
            }

            var club = await _context.Clubs.FindAsync(Club.ClubId);
            if (club == null)
            {
                return NotFound();
            }

            club.ClubName = Club.ClubName;
            club.Address = Club.Address;
            club.ClubEmail = Club.ClubEmail;
            club.ClubPhone = Club.ClubPhone;
            club.FanpageLink = Club.FanpageLink;
            club.AvatarLink = Club.AvatarLink;
            club.OpenTime = Club.OpenTime;
            club.CloseTime = Club.CloseTime;
            club.DistrictId = DistrictId;

            try
            {
                await _context.SaveChangesAsync();
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

            return RedirectToPage("./ClubManage");
        }

        public async Task<JsonResult> OnGetGetClubDetails(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.District)
                .FirstOrDefaultAsync(c => c.ClubId == id);

            if (club == null)
            {
                return new JsonResult(null);
            }

            return new JsonResult(new
            {
                club.ClubName,
                club.Address,
                CityId = club.District.CityId,
                club.DistrictId,
                club.ClubEmail,
                club.ClubPhone,
                club.FanpageLink,
                club.AvatarLink,
                OpenTime = club.OpenTime.HasValue ? club.OpenTime.Value.ToString("hh\\:mm") : "",
                CloseTime = club.CloseTime.HasValue ? club.CloseTime.Value.ToString("hh\\:mm") : ""
            });
        }


        public async Task<JsonResult> OnGetGetDistricts(int cityId)
        {
            var districts = await _context.Districts.Where(d => d.CityId == cityId).Select(d => new SelectListItem
            {
                Value = d.DistrictId.ToString(),
                Text = d.DistrictName
            }).ToListAsync();

            return new JsonResult(districts);
        }

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
