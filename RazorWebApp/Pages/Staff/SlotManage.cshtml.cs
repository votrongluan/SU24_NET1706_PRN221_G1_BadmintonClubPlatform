using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using BusinessObjects.Enums;

namespace RazorWebApp.Pages.Staff
{
    public class SlotManageModel : AuthorPageServiceModel
    {
        private readonly BusinessObjects.Entities.BcbpContext _context;

        public SlotManageModel(BusinessObjects.Entities.BcbpContext context)
        {
            _context = context;
        }

        public IList<Slot> Slots { get; set; }
        public List<Club> AllClubs { get; set; }
 

        public async Task<IActionResult> OnGetAsync()
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            Slots = _context.Slots
            .Include(s => s.Club)
                .ToList();
            AllClubs = _context.Clubs.ToList();

            return Page();
        }

        [BindProperty]
        public Slot Slot { get; set; }

        public IActionResult OnPostSaveSlot()
        {
            try
            {
                _context.Slots.Add(Slot);
                _context.SaveChanges();

                return Page();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
