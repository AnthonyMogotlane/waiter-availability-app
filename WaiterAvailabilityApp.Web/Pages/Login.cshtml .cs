﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IWaiterAvailability _waiter;
    public LoginModel(ILogger<LoginModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    [BindProperty]
    public Waiter waiter { get; set; }   

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            ModelState.Clear();
            if(waiter.FirstName == "Admin")
            {
                return Redirect($"/Schedule/?FirstName={waiter.FirstName}");
            }
            else
            {
                return Redirect($"/?FirstName={waiter.FirstName}");
            }
        }
        return Page();
    }
}