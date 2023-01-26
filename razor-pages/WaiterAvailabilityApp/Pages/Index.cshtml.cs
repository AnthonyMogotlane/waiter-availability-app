using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WaiterAvailabilityApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWaiterAvailability _waiter;

    [BindProperty]
    public List<int> SelectedDays { get; set; } // To hold selected days by the waiter

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
    }

    public void OnPost()
    {
        _waiter.AddToSchedule(SelectedDays);
    }
}
