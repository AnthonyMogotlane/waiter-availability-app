using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp;

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

    // Schedule - weekly waiter shift
    public Dictionary<string, List<string>> Schedule { get; set; }

    public void OnGet()
    {  
        _waiter.GetData();
        Schedule = _waiter.GetSchedule();
         Console.WriteLine(Schedule["Monday"].Count);
    }

    public IActionResult OnPostClear()
    {
        _waiter.ClearSchedule(); // Clear from the database
        _waiter.ClearLocalData(); // Clear local store data
        _waiter.GetData();
        Schedule = _waiter.GetSchedule();
        return Page();
    }
}
