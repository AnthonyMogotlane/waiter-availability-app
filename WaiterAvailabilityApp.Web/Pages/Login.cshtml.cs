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
    public string? FirstName { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public IActionResult OnPost()
    {
        System.Console.WriteLine(FirstName);

        if (ModelState.IsValid)
        {

            ModelState.Clear();
            if (Waiter.FirstName! == "Admin")
            {
                // Set session value
                HttpContext.Session.SetString("_FirstName", Waiter.FirstName!);
                return Redirect($"/Schedule");
            }
            else
            {
                // Check if name is registered
                if (_waiter.CheckWaiter(Waiter.FirstName!))
                {
                    // Set session value
                    HttpContext.Session.SetString("_FirstName", Waiter.FirstName!);
                    return Redirect($"/Waiter");
                }
                else
                {
                    Waiter.FirstName = "";
                    TempData["invalidName"] = "You have Enter invalid login name";
                }
            }
        }
        return Page();
    }
}
