namespace WebAppRazor.Pages
{
    public class AboutModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
