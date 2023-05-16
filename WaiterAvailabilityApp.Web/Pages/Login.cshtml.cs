using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;

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
    public Waiter Waiter { get; set; }
    [BindProperty]
    public Waiter Password { get; set; }
    public string? FirstName { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public IActionResult OnPost()
    {
        if (_waiter.CheckValidUser("Admin", Waiter.Password!))
        {
            // Set session value
            HttpContext.Session.SetString("_FirstName", Waiter.FirstName!);
            return Redirect($"/Schedule");
        }
        else
        {
            // Check if name is registered
            if (_waiter.CheckValidUser(Waiter.FirstName!, Waiter.Password!))
            {
                // Set session value
                HttpContext.Session.SetString("_FirstName", Waiter.FirstName!);
                return Redirect($"/Waiter");
            }
            else
            {
                TempData["invalidName"] = "You have entered invalid login name or password";
                Waiter.FirstName = "";
            }
        }
        return Page();
    }
}
