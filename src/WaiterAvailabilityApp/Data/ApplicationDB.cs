using Dapper;
using Npgsql;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp.Data;
public class ApplicationDB : IWaiterAvailability
{
    private string? ConnectionString { get; set; }

    private string? Name { get; set; } // Name of the current user
    private List<string> WorkingDays { get; set; }

    // Day of the week
    private static List<string> monday = new List<string>();
    private static List<string> tuesday = new List<string>();
    private static List<string> wednesday = new List<string>();
    private static List<string> thursday = new List<string>();
    private static List<string> friday = new List<string>();
    private static List<string> saturday = new List<string>();
    private static List<string> sunday = new List<string>();

    // List of workers on a specific day
    private Dictionary<string, List<string>>? schedule = new Dictionary<string, List<string>>()
    {
        {"Monday", monday},
        {"Tuesday", tuesday},
        {"Wednesday", wednesday},
        {"Thursday", thursday},
        {"Friday", friday},
        {"Saturday", saturday},
        {"Sunday", sunday},
    };
    public ApplicationDB(string connectionString)
    {
        this.ConnectionString = connectionString;
        this.WorkingDays = new List<string>();
    }

    public void CurrentUser(string name) => this.Name = name; // Setting the name of the current user/waiter
    public string GetName() => Name!;
    public List<string> GetWorkingDays() => WorkingDays;

    // Add to Schedule table
    public void AddToSchedule(List<int> checkedDays)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        var result = connection.QueryFirst(
            @"SELECT * FROM waiters WHERE firstname = @FirstName",
            new { FirstName = Name }); // DapperRow

        int waiterId = result.id;

        foreach (var day in checkedDays)
        {
            connection.Execute(
                @"INSERT INTO schedule (day_id, waiter_id) VALUES (@DayId, @WaiterId)",
                new { DayId = day, WaiterId = waiterId });
        }

    }

    // Retrive data from Schedule table
    public void GetData()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        var sql = @"SELECT w.firstname, wd.id, wd.day FROM weekdays wd
                INNER JOIN schedule s ON wd.id = s.day_id
                INNER JOIN waiters w ON w.id = s.waiter_id";

        var result = connection.Query<Schedule>(sql);

        // Adding data from DB to schedule
        foreach (var item in result.ToList())
        {
            if (!schedule![item.Day!].Contains(item.FirstName!))
                schedule![item.Day!].Add(item.FirstName!);
        }

        foreach (var item in result.ToList())
        {
            if (item.FirstName == this.Name && !WorkingDays.Contains(item.Day!))
                WorkingDays.Add(item.Day!);
        }

    }

    public void ClearWorkingDays() => WorkingDays.Clear();

    public Dictionary<string, List<string>> GetSchedule() => schedule!;
    public void ClearSchedule()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        connection.Execute(@"DELETE FROM schedule");
    }

    // Adding a new waiter into the database.
    public void AddName(string name)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        connection.Execute(@"INSERT INTO waiters (firstname) VALUES (@FirstName)", new { FirstName = name });
    }

    // Clear local days data
    public void ClearLocalData()
    {
        foreach (var day in schedule!)
        {
            day.Value.Clear();
        }
    }

    // Delete waiter's selected days
    public string DeleteDays(string name)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        var result = connection.QueryFirst(
            @"SELECT * FROM waiters WHERE firstname = @FirstName",
            new { FirstName = name }); // DapperRow

        int waiterId = result.id;

        connection.Execute(@"DELETE FROM schedule WHERE waiter_id = @WaiterId", new { WaiterId = waiterId});

        return $"{name}'s selected days deleted";
    }
}