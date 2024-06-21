namespace WebAppRazor.Pages
{
    public class MatchModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
