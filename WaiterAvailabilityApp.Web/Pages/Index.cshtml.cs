using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WaiterAvailabilityApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Clear session for the current user
            HttpContext.Session.Clear();
        }
    }
}