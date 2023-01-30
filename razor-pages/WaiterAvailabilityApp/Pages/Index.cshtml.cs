using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWaiterAvailability _waiter;

    [BindProperty(SupportsGet = true)]
    public Waiter Waiter { get; set; }

    [BindProperty]
    public string WaiterName {get;set;}


    [BindProperty]
    public List<int> SelectedDays { get; set; } // To hold selected days by the waiter
    public List<string> WorkingDays { get; set; }
    public Dictionary<string, List<string>> Schedule { get; set; }

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

    public IndexModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    public void Execute()
    {
        _waiter.CurrentUser(Waiter.FirstName!);
        _waiter.ClearWorkingDays();
        _waiter.GetData();
        WorkingDays = _waiter.GetWorkingDays();
        Schedule = _waiter.GetSchedule();
    }

    public void OnGet()
    {
        Execute();
    }

    public void OnPost()
    {
        _waiter.DeleteDays(Waiter.FirstName!); // If there are days selected, delete them to allow update.
        _waiter.AddToSchedule(SelectedDays);
        Execute();
    }

    public IActionResult OnPostReset()
    {
        _waiter.DeleteDays(WaiterName);
        Execute();
        return Redirect($"/?FirstName={WaiterName}");
    }
}
