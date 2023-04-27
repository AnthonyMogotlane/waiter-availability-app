using WaiterAvailabilityApp.Model;
namespace WaiterAvailabilityApp;
public interface IWaiterAvailability
{
    void AddName(string name, string password);
    void AddToSchedule(string firstName, List<int> checkedDays);
    List<Schedule> GetSchedule();
    void ClearSchedule();
    void ResertDays(string name);
    void ResertDates(string name, Dictionary<DateOnly, DayOfWeek> dates);
    List<Weekday> GetWeekdays();
    List<Weekday> WaiterWorkingDays(string name);
    bool CheckUsername(string name);
    bool CheckValidUser(string name, string password);
    void AddToScheduleWithDates(string firstName, List<string> checkedDays);
    List<Schedule> WaiterWorkingDates(string name);
     public void ClearCurrentWeek(Dictionary<DateOnly, DayOfWeek> dates);
}