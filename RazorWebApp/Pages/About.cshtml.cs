using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages
{
    public class AboutModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
