using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages
{
    public class ClubDetailModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
