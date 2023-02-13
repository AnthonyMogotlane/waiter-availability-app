using Dapper;
using Npgsql;
using WaiterAvailabilityApp.Model;

namespace WaiterAvailabilityApp;
public class WaiterAvailability : IWaiterAvailability
{
    private string? ConnectionString { get; set; }

    public WaiterAvailability(string connectionString) => this.ConnectionString = connectionString;

    // Add to Schedule table
    // List of days represented by numbers, 1 to 7
    public void AddToSchedule(string firstName, List<int> checkedDays)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            var result = connection.QueryFirst(
                @"SELECT * FROM waiters WHERE firstname = @FirstName",
                new { FirstName = firstName });

            int waiterId = result.id;

            foreach (var day in checkedDays)
            {
                connection.Execute(
                    @"INSERT INTO schedule (day_id, waiter_id) VALUES (@DayId, @WaiterId)",
                    new { DayId = day, WaiterId = waiterId });
            }
        };
    }

    public List<Schedule> GetSchedule()
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Schedule>(@"
                    SELECT w.firstname, wd.day FROM weekdays wd
                    INNER JOIN schedule s ON wd.id = s.day_id
                    INNER JOIN waiters w ON w.id = s.waiter_id").ToList();
        }
    }

    public void ClearSchedule()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Execute(@"DELETE FROM schedule");
        }
    }

    public void AddName(string name)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Execute(@"INSERT INTO waiters (firstname) VALUES (@FirstName)", new { FirstName = name });
        }
    }

    public List<Waiter> GetWaiters()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Waiter>(@"SELECT * FROM waiters").ToList();
        }
    }

    // Delete previously selected days for a waiter
    public void ResertDays(string name)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
        {
            var result = connection.QueryFirst(
                @"SELECT * FROM waiters WHERE firstname = @FirstName",
                new { FirstName = name });

            int waiterId = result.id;

            connection.Execute(@"DELETE FROM schedule WHERE waiter_id = @WaiterId", new { WaiterId = waiterId });
        }
    }

    public List<Weekday> GetWeekdays()
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Weekday>(@"SELECT * FROM weekdays").ToList();
        }
    }

    public List<Weekday> WaiterWorkingDays(string name)
    {
        using(var connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Weekday>(@"
                    SELECT wd.id, wd.day, w.firstname FROM weekdays wd
                    INNER JOIN schedule s ON wd.id = s.day_id
                    INNER JOIN waiters w ON w.id = s.waiter_id
                    WHERE w.firstname = @FirstName", new { FirstName = name}).ToList();
        }
    }

    public bool CheckWaiter(string name)
    {
        using(var connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Waiter>(@"SELECT * FROM waiters
                WHERE firstname = @FirstName", new {FirstName = name}).ToList().Count > 0 ? true : false;
            
        }
    }

    public bool CheckValidUser(string name, string password)
    {
         using(var connection = new NpgsqlConnection(ConnectionString))
        {
            return connection.Query<Waiter>(@"SELECT * FROM waiters
                WHERE firstname = @FirstName and password = @Password" , new {FirstName = name, Password = password}).ToList().Count > 0 ? true : false;
            
        }
    }
}