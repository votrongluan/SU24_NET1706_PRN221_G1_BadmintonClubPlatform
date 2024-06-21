namespace WebAppRazor.Pages
{
    public class ServiceModel : AuthorPageServiceModel
    {
        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
