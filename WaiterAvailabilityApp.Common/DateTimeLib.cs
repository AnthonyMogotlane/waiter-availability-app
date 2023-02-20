namespace WaiterAvailabilityApp.Lib;
static public class DateTimeLib
{
    // Store days and dates in a dictionary
    static public Dictionary< DateOnly, DayOfWeek> ListOfWeekDayAndDates(DateTime currentDateTime, int start, int end)
    {
        var listOfWeekDayAndDates = new Dictionary< DateOnly, DayOfWeek>();
        // Add dates to a list
        for (int i = start; i < end; i++)
            listOfWeekDayAndDates.Add(
                DateOnly.FromDateTime(DateTime.Now.AddDays((DayOfWeek.Monday - currentDateTime.DayOfWeek) + i)),
                DateTime.Now.AddDays((DayOfWeek.Monday - currentDateTime.DayOfWeek) + i).DayOfWeek);

        return listOfWeekDayAndDates;
    }
}