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
        // _waiter.ClearSchedule();
        _waiter.GetData();
        Schedule = _waiter.GetSchedule();
    }

    public void OnPost()
    {
        _waiter.ClearSchedule();
        Schedule = _waiter.GetSchedule();
    }
}
