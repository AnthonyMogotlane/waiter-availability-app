using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages;

public class WaiterModel : PageModel
{
    private readonly ILogger<WaiterModel> _logger;
    private readonly IWaiterAvailability _waiter;

    public string? FirstName { get; set; }

    [BindProperty]
    public string WaiterFirstName { get; set; }


    [BindProperty]
    public List<int> SelectedDays { get; set; } // To hold selected days by the waiter
    public IEnumerable<string> WaiterWorkingDays { get; set; }
    public IEnumerable<IGrouping<string?, Schedule>> Schedule { get; set; }
    public Dictionary<string, int> WeekDayStatus { get; set; }


    public Dictionary<int, string> weekdays = new Dictionary<int, string>()
    {
        {1, "Monday"},
        {2, "Tuesday"},
        {3, "Wednesday"},
        {4, "Thursday"},
        {5, "Friday"},
        {6, "Saturday"},
        {7, "Sunday"}
    };

    public WaiterModel(ILogger<WaiterModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    // Get how many waiters have booked for that day
    public void GetWeekDayStatus()
    {
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        WeekDayStatus = new Dictionary<string, int>();
        foreach (var group in Schedule) WeekDayStatus.Add(group.Key!, group.Count());
    }

    public void OnGet()
    {
        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        WaiterWorkingDays = _waiter.WaiterWorkingDays(FirstName!).Select(x => x.Day)!;
        GetWeekDayStatus();
    }

    public IActionResult OnPost()
    {
        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        if (FirstName == null)
        {
            WaiterWorkingDays = _waiter.WaiterWorkingDays(FirstName!).Select(x => x.Day)!;
            GetWeekDayStatus();
            TempData["login"] = "login";
            return Page();
        }
        else if (SelectedDays.Count > 0 && SelectedDays.Count <= 5)
        {
            _waiter.ResertDays(FirstName!);
            _waiter.AddToSchedule(FirstName!, SelectedDays);
            WaiterWorkingDays = _waiter.WaiterWorkingDays(FirstName!).Select(x => x.Day)!;
            GetWeekDayStatus();
            TempData["submit"] = "Days submitted successfully";
            return Page();
        }
        else
        {
            WaiterWorkingDays = _waiter.WaiterWorkingDays(FirstName!).Select(x => x.Day)!;
            GetWeekDayStatus();
            TempData["message"] = "Minimum days to work is 1, Maximum days to work is 5";
            return Page();
        }
    }

    public IActionResult OnPostReset()
    {
        _waiter.ResertDays(WaiterFirstName);
        WaiterWorkingDays = _waiter.WaiterWorkingDays(WaiterFirstName).Select(x => x.Day)!;
        TempData["reset"] = "Days reseted successfully";

        return Redirect($"/Waiter");
    }
}
