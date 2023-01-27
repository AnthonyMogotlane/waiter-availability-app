using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityAppRazor.Models;

namespace WaiterAvailabilityApp.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IWaiterAvailability _waiter;
    public LoginModel(ILogger<LoginModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    [BindProperty]
    public Waiter waiter { get; set; }   

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            return Redirect($"/?FirstName={waiter.FirstName}");
        }
            ModelState.Clear();

        return Page();
    }
}
