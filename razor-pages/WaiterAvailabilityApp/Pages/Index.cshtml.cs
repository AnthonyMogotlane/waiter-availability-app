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
    public string WaiterFirstName {get;set;}


    [BindProperty]
    public List<int> SelectedDays { get; set; } // To hold selected days by the waiter
    public IEnumerable<string> WaiterWorkingDays { get; set; }
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

    public void OnGet()
    {
        WaiterWorkingDays = _waiter.WaiterWorkingDays(Waiter.FirstName!).Select(x => x.Day)!;
    }

    public void OnPost()
    {
        _waiter.ResertDays(Waiter.FirstName!);
        _waiter.AddToSchedule(Waiter.FirstName!, SelectedDays);
        WaiterWorkingDays = _waiter.WaiterWorkingDays(Waiter.FirstName!).Select(x => x.Day)!;
    }

    public IActionResult OnPostReset()
    {
        _waiter.ResertDays(WaiterFirstName);    
        WaiterWorkingDays = _waiter.WaiterWorkingDays(WaiterFirstName).Select(x => x.Day)!;
        return Redirect($"/?FirstName={WaiterFirstName}");
    }
}
