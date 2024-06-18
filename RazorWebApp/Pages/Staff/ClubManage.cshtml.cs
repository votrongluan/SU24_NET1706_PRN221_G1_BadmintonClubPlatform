using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessObjects;
using Services.IService;
using System.Text.Json;
using BusinessObjects.Enums;

namespace RazorWebApp.Pages.Staff
{
    public class ClubManageModel : AuthorPageServiceModel
    {
        private readonly BusinessObjects.Entities.BcbpContext _context;
        private readonly IClubService _clubService;

        public ClubManageModel(BusinessObjects.Entities.BcbpContext context, IClubService clubService)
        {
            _context = context;
            _clubService = clubService;
        }

        [BindProperty]
        public Club Club { get; set; }

        [BindProperty]
        public int CityId { get; set; }

        [BindProperty]
        public int DistrictId { get; set; }

        public SelectList Cities { get; set; }
        public SelectList Districts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Lấy thông tin tài khoản từ session
            string accountJson = HttpContext.Session.GetString("Account");
            if (accountJson == null)
            {
                return RedirectToPage("../Authentication/Login");
            }

            Account account = JsonSerializer.Deserialize<Account>(accountJson);

            int id = (int)account.ClubManageId;
            
            if (id != 0) 
            {
                Club = _clubService.GetClubById(id);
            }

            if (Club == null)
            {
                return NotFound();
            }

            CityId = Club.District.CityId;
            DistrictId = Club.DistrictId;

            Cities = new SelectList(await _context.Cities.ToListAsync(), "CityId", "CityName");
            Districts = new SelectList(await _context.Districts.Where(d => d.CityId == CityId).ToListAsync(), "DistrictId", "DistrictName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy thông tin tài khoản từ session
            string accountJson = HttpContext.Session.GetString("Account");
            if (accountJson == null)
            {
                return RedirectToPage("/Authentication");
            }

            Account account = JsonSerializer.Deserialize<Account>(accountJson);

            var club = await _context.Clubs.FindAsync(account.ClubManageId);
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
                TempData["SuccessMessage"] = "Đã cập nhật thành công";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(club.ClubId))
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

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
