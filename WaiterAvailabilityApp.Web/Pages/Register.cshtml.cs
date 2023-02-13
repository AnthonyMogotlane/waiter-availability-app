using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWaiterAvailability _waiter;
        [BindProperty]
        public Waiter Waiter { get; set; }

        public RegisterModel(ILogger<RegisterModel> logger, IWaiterAvailability waiter)
        {
            _logger = logger;
            _waiter = waiter;
        }
        public void OnPost()
        {
            if(ModelState.IsValid)
            {
                if(!_waiter.CheckWaiter(Waiter.FirstName!))
                {
                    _waiter.AddName(Waiter.FirstName!);
                    TempData["registration"] = "Registration Successful";
                }
                else
                {
                    TempData["nameUnavailable"] = "UserName not available, try another name";
                }
                Waiter.FirstName = "";
                ModelState.Clear();
            }
        }
    }
}
