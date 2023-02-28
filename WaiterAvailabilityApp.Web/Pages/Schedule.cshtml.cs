using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Lib;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages;
public class ScheduleModel : PageModel
{
    private readonly ILogger<ScheduleModel> _logger;
    private IWaiterAvailability _waiter;
    public ScheduleModel(ILogger<ScheduleModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    public string? FirstName { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Start { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Week { get; set; }
    public int End { get; set; }

    [TempData]
    public string? View { get; set; }

    public Dictionary<DateOnly, DayOfWeek> weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, 0, 7);
    public IEnumerable<IGrouping<string?, Schedule>>? Schedule { get; set; }

    public void OnGet()
    {
        FirstName = HttpContext.Session.GetString("_FirstName");

        if (FirstName != null)
        {
            End = Start + 7;

            if (Start != 0 && Start % 7 == 0)
            {
                weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, Start, End);
            }
            if (Start == 0 || Week == 0)
            {
                DateTimeLib.Start = 0;
                DateTimeLib.GetCurrentWeek();
                Week = DateTimeLib.Week;
            }
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Dates);
        }
        else
        {
            TempData["login"] = "Please login first to see the schedule";
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Dates);
        }
    }

    public IActionResult OnPostClear()
    {
        if (HttpContext.Session.GetString("_FirstName") != null)
        {
            _waiter.ClearSchedule(); // Clear from the database
            Schedule = _waiter.GetSchedule().GroupBy(x => x.Dates);
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

    // Move to previous week
    public IActionResult OnPostPrev()
    {
        if (DateTimeLib.Start > 0)
        {
            DateTimeLib.DecrementStart();
            DateTimeLib.DecrementWeek();
        }
        return Redirect($"/Schedule?start={DateTimeLib.Start}&week={DateTimeLib.Week}");
    }

    // Move to following week
    public IActionResult OnPostNext()
    {
        if (DateTimeLib.Start < 21)
        {
            DateTimeLib.IncrementStart();
            DateTimeLib.IncrementWeek();
        }
        return Redirect($"/Schedule?start={DateTimeLib.Start}&week={DateTimeLib.Week}");
    }
}
