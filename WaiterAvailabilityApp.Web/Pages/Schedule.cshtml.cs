using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages;
public class ScheduleModel : PageModel
{
    private readonly ILogger<ScheduleModel> _logger;
    private IWaiterAvailability _waiter;
    public string? FirstName { get; set; }
    public ScheduleModel(ILogger<ScheduleModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    [TempData]
    public string View { get; set; }

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
        FirstName = HttpContext.Session.GetString("_FirstName");

        if (FirstName != null)
        {
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        }
        else
        {
            TempData["login"] = "Please login first to see the schedule";
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        }
    }

    public IActionResult OnPostClear()
    {
        if (HttpContext.Session.GetString("_FirstName") != null)
        {
            _waiter.ClearSchedule(); // Clear from the database
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
            TempData["success"] = "Schedule cleared successfully";
            return Page();
        }
        else
        {
            TempData["login"] = "Please login first to see the schedule";
            return Page();
        }
    }

    public IActionResult OnPostAccount(string name)
    {
        if (HttpContext.Session.GetString("_FirstName") != null)
        {
            HttpContext.Session.SetString("_WaiterAccountName", name);
            return Redirect($"/Admin/");
        }
        return Page();
    }

    public IActionResult OnPostTable()
    {
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        View = "Table";
        return Page();
    }
    public IActionResult OnPostBoard()
    {
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        View = "Board";
        return Page();
    }
}
