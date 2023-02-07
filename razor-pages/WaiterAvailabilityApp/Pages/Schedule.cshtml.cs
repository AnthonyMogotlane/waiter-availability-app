using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Model;


namespace WaiterAvailabilityApp.Pages;

public class ScheduleModel : PageModel
{
    private readonly ILogger<ScheduleModel> _logger;
    private IWaiterAvailability _waiter;

    [BindProperty(SupportsGet = true)]
    public Waiter Waiter { get; set; }
    public ScheduleModel(ILogger<ScheduleModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    public List<string> weekdays = new List<string>()
    {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday"
    };

    // Schedule - weekly waiter shift
    public IEnumerable<IGrouping<string?, Schedule>> Schedule { get; set; }

    public void OnGet()
    {  
        if(Waiter.FirstName != null)
        {
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        }
        else
        {
            TempData["login"] = "login";
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        }
    }

    public IActionResult OnPostClear()
    {
        _waiter.ClearSchedule(); // Clear from the database
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        TempData["success"] = "Schedule cleared successfully";
        return Page();
    }

    public IActionResult OnPostAccount(string name)
    {
        return Redirect($"/Admin/?FirstName={name}");
    }
}
