﻿using Microsoft.AspNetCore.Mvc;
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
    public IEnumerable<string> WaiterWorkingDates { get; set; }
    public List<string> CurrentWorkingDates { get; set; }
    public IEnumerable<IGrouping<string?, Schedule>> Schedule { get; set; }
    public Dictionary<string, int> WeekDayStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Start { get; set; }
    public int End { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Week { get; set; }
    public Dictionary<DateOnly, DayOfWeek> weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, 0, 7);
    [TempData]
    public string WeekLimits { get; set; }

    public WaiterModel(ILogger<WaiterModel> logger, IWaiterAvailability waiter)
    {
        _logger = logger;
        _waiter = waiter;
    }

    // Get how many waiters have booked for that day/date
    public void GetWeekDayStatus()
    {
        Schedule = _waiter.GetSchedule().GroupBy(x => x.Dates);
        WeekDayStatus = new Dictionary<string, int>();
        foreach (var group in Schedule) WeekDayStatus.Add(group.Key!, group.Count());
    }
    public void GetWaitersWorkingDates() =>
        WaiterWorkingDates = _waiter.WaiterWorkingDates(FirstName!).Select(x => x.Dates)!;

    // Reset current state selected dates method
    public void ResertDates()
    {
        End = DateTimeLib.Start + 7;
        weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, DateTimeLib.Start, End);
        _waiter.ResertDates(WaiterFirstName, weekdays);
    }

    // Filter selected days for the current state
    public void GetCurrentWorkingDates()
    {
        WaiterWorkingDates = _waiter.WaiterWorkingDates(FirstName!).Select(x => x.Dates)!;
        weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, DateTimeLib.Start, End);
        List<string> listOfDates = new List<string>();

        foreach (var item in WaiterWorkingDates)
        {
            if(weekdays.ContainsKey(DateOnly.FromDateTime(DateTime.Parse(item))))
            {
                listOfDates.Add(item);
            }
        }

        CurrentWorkingDates = listOfDates;
    }

    public void OnGet()
    {
        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        // End of the week to be 7 days from start
        End = Start + 7;

        if (Start != 0 && Start % 7 == 0)
        {
            weekdays = DateTimeLib.ListOfWeekDayAndDates(DateTime.Now, Start, End);
        }
        if (Start == 0)
        {
            DateTimeLib.Start = 0;
            DateTimeLib.GetCurrentWeek();
            Week = DateTimeLib.Week;
        }

        // Selected waiter working days
        GetWaitersWorkingDates();
        GetCurrentWorkingDates();
        // Status of the day
        GetWeekDayStatus();
    }

    public IActionResult OnPost()
    {

        // Get session value
        FirstName = HttpContext.Session.GetString("_FirstName");

        if (FirstName == null)
        {
            GetWaitersWorkingDates();
            GetCurrentWorkingDates();
            GetWeekDayStatus();
            TempData["login"] = "login";
            return Page();
        }
        else if (SelectedDays.Count > 0 && SelectedDays.Count <= 5)
        {
            // Remove selected days for current state
            ResertDates();

            _waiter.AddToScheduleWithDates(FirstName!, SelectedDays);
            GetWaitersWorkingDates();
            GetCurrentWorkingDates();
            GetWeekDayStatus();
            TempData["submit"] = "Days submitted successfully";
            return Page();
        }
        else
        {
            GetWaitersWorkingDates();
            GetCurrentWorkingDates();
            GetWeekDayStatus();
            TempData["message"] = "Minimum days to work is 1, Maximum days to work is 5";
            return Page();
        }
    }

    public IActionResult OnPostReset()
    {
        // Remove selected days for current state
        ResertDates();

        GetWaitersWorkingDates();
        GetCurrentWorkingDates();
        TempData["reset"] = "Days reseted successfully";

        return Redirect($"/Waiter?start={DateTimeLib.Start}&week={DateTimeLib.Week}");
    }

    // Move to previous week
    public IActionResult OnPostPrev()
    {
        if (DateTimeLib.Start > 0)
        {
            DateTimeLib.DecrementStart();
            DateTimeLib.DecrementWeek();
        }
        else
        {
            WeekLimits = $"You can only select your working days from week {DateTimeLib.Week}";
        }

        return Redirect($"/Waiter?start={DateTimeLib.Start}&week={DateTimeLib.Week}");
    }

    // Move to following week
    public IActionResult OnPostNext()
    {
        if (DateTimeLib.Start < 21)
        {
            DateTimeLib.IncrementStart();
            DateTimeLib.IncrementWeek();
        }
        else
        {
            WeekLimits = $"You can only select your working days till week {DateTimeLib.Week}";
        }

        return Redirect($"/Waiter?start={DateTimeLib.Start}&week={DateTimeLib.Week}");
    }
}
