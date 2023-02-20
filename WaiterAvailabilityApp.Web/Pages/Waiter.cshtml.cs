using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;
using WaiterAvailabilityApp.Lib;

namespace WaiterAvailabilityApp.Pages;

public class WaiterModel : PageModel
{
    private readonly ILogger<WaiterModel> _logger;
    private readonly IWaiterAvailability _waiter;

    public string? FirstName { get; set; }

    [BindProperty]
    public string WaiterFirstName { get; set; }


    [BindProperty]
    public List<string> SelectedDays { get; set; } // To hold selected days by the waiter
    public IEnumerable<string> WaiterWorkingDays { get; set; }
    public IEnumerable<string> WaiterWorkingDates { get; set; }
    public IEnumerable<IGrouping<string?, Schedule>> Schedule { get; set; }
    public Dictionary<string, int> WeekDayStatus { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public Dictionary<DateOnly, DayOfWeek> weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, 0, 7);


    public WaiterModel(ILogger<WaiterModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    // Get how many waiters have booked for that day
    public void GetWeekDayStatus()
    {
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Dates);
        WeekDayStatus = new Dictionary<string, int>();
        foreach (var group in Schedule) WeekDayStatus.Add(group.Key!, group.Count());
    }
    public void GetWaitrsWorkingDates() => 
        WaiterWorkingDates = _waiter.WaiterWorkingDates(FirstName!).Select(x => x.Dates)!;

    public void OnGet()
    {
        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        GetWaitrsWorkingDates();
        GetWeekDayStatus();
    }

    public IActionResult OnPost()
    {
        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        if (FirstName == null)
        {
            GetWaitrsWorkingDates();
            GetWeekDayStatus();
            TempData["login"] = "login";
            return Page();
        }
        else if (SelectedDays.Count > 0 && SelectedDays.Count <= 5)
        {
            _waiter.ResertDays(FirstName!);
            _waiter.AddToScheduleWithDates(FirstName!, SelectedDays);
            GetWaitrsWorkingDates();
            GetWeekDayStatus();
            TempData["submit"] = "Days submitted successfully";
            return Page();
        }
        else
        {
            GetWaitrsWorkingDates();
            GetWeekDayStatus();
            TempData["message"] = "Minimum days to work is 1, Maximum days to work is 5";
            return Page();
        }
    }

    public IActionResult OnPostReset()
    {
        _waiter.ResertDays(WaiterFirstName);
        GetWaitrsWorkingDates();
        TempData["reset"] = "Days reseted successfully";

        return Redirect($"/Waiter");
    }

    public IActionResult OnPostNext()
    {
        Start = 7;
        End = 14;
        
        HttpContext.Session.SetString("_FirstName", WaiterFirstName);
        System.Console.WriteLine(HttpContext.Session.GetString("_FirstName"));
        weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, 7, 14);
        GetWeekDayStatus();
        GetWaitrsWorkingDates();
       
        return Redirect($"/Waiter");
    }
}
