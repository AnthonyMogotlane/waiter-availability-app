using System.Globalization;

namespace WaiterAvailabilityApp.Lib;
static public class DateTimeLib
{
    // Gets the Calendar instance associated with a CultureInfo.
    static CultureInfo cultureInfo = new CultureInfo("en-US");
    static Calendar calendar = cultureInfo.Calendar;

    // Gets the DTFI properties required by GetWeekOfYear.
    static CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
    static DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
    // Get week number
    public static int Week { get; set; } = calendar.GetWeekOfYear( DateTime.Now, calendarWeekRule, firstDayOfWeek);
    static public int Start { get; set; }


    // Store days and dates in a dictionary
    static public Dictionary<DateOnly, DayOfWeek> ListOfWeekDayAndDates(DateTime currentDateTime, int start, int end)
    {
        var listOfWeekDayAndDates = new Dictionary<DateOnly, DayOfWeek>();
        // Add dates to a list
        for (int i = start; i < end; i++)
            listOfWeekDayAndDates.Add(
                DateOnly.FromDateTime(DateTime.Now.AddDays((DayOfWeek.Monday - currentDateTime.DayOfWeek) + i)),
                DateTime.Now.AddDays((DayOfWeek.Monday - currentDateTime.DayOfWeek) + i).DayOfWeek);

        return listOfWeekDayAndDates;
    }

    static public void IncrementStart() => Start += 7;
    static public void DecrementStart() => Start -= 7;
    static public void IncrementWeek() => Week += 1;
    static public void DecrementWeek() => Week -= 1;
}