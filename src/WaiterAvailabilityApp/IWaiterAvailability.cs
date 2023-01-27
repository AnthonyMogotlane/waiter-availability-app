namespace WaiterAvailabilityApp;
public interface IWaiterAvailability
{
    public void AddName(string name);

    public void CurrentUser(string name);
    public string GetName();
    public List<string> GetWorkingDays();
    public void AddToSchedule(List<int> selectedDays);
    public void GetData();
    Dictionary<string, List<string>> GetSchedule();
    public void ClearSchedule();
    public void ClearWorkingDays();
}