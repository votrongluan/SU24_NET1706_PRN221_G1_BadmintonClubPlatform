using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages
{
    public class ServiceModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
