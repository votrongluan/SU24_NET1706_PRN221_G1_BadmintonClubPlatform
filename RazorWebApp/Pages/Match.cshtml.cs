using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages
{
    public class MatchModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
