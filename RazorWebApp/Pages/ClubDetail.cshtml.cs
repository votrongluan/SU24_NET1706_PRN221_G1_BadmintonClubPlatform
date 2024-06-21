namespace WebAppRazor.Pages
{
    public class ClubDetailModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
