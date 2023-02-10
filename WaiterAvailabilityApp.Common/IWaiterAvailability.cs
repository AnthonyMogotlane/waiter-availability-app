using WaiterAvailabilityApp.Model;
namespace WaiterAvailabilityApp;
public interface IWaiterAvailability
{
    void AddName(string name);
    void AddToSchedule(string firstName, List<int> checkedDays);
    List<Schedule> GetSchedule();
    void ClearSchedule();
    void ResertDays(string name);
    List<Weekday> GetWeekdays();
    List<Weekday> WaiterWorkingDays(string name);
    bool CheckWaiter(string name);
}