using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace RazorWebApp.Pages
{
    public class TestModel : PageModel
    {
        public List<string> Data { get; set; }
        public List<string> Results { get; set; }
        public string SearchTerm { get; set; }

        public TestModel()
        {
            // Initialize the static list of data
            Data = new List<string>
            {
                "Apple",
                "Banana",
                "Orange",
                "Pineapple",
                "Grape",
                "Mango",
                "Blueberry",
                "Strawberry"
            };
        }

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                // Perform the search
                Results = Data.Where(item => item.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                Results = new List<string>();
            }
        }
    }
}