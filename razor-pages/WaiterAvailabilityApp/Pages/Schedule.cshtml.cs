using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp;
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

    // Schedule - weekly waiter shift
    public IEnumerable<IGrouping<string?, Schedule>> Schedule { get; set; }

    public void OnGet()
    {  
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
    }

    public IActionResult OnPostClear()
    {
        _waiter.ClearSchedule(); // Clear from the database
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Day);
        return Page();
    }
}
