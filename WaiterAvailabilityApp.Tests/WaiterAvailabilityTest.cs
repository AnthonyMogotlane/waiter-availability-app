using System.Data.Common;
using System.Text.Json;
using Dapper;
using Npgsql;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Model;
using Xunit;

namespace WaiterAvailabilityApp.Tests;

public class WaiterAvailabilityTest
{
    static string cs = "Host=localhost;Username=postgres;Password=0000;Database=test_db;IncludeErrorDetail=true";
    static string GetConnectionString()
    {
        var csEnv = Environment.GetEnvironmentVariable("PSQLConnectionString");
        if (csEnv == null) csEnv = cs;
        return csEnv;
    }

    public void DropTables()
    {
        using (var connection = new NpgsqlConnection(GetConnectionString()))
        {
            connection.Execute(File.ReadAllText("../../../../sql/dropTables.sql"));
        }
    }

    public void CreateTables()
    {
        using (var connection = new NpgsqlConnection(GetConnectionString()))
        {
            connection.Execute(File.ReadAllText("../../../../sql/tables.sql"));
        }
    }


    [Fact]
    public void ShouldBeAbleToAddWaitersName()
    {
        // Given
        var waiterAvailability = new WaiterAvailability(GetConnectionString());
        DropTables();
        CreateTables();
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        waiterAvailability.AddName("Fabiano", "fabiano123");
        var expected = connection.Query<Waiter>(@"SELECT * FROM waiters").ToList();

        // When 
        var actual = waiterAvailability.GetWaiters();

        // Then 
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ShouldBeAbleToReturnTheCountOfWaitersInWaitersTable()
    {
        // Given
        var waiterAvailability = new WaiterAvailability(GetConnectionString());
        DropTables();
        CreateTables();
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        waiterAvailability.AddName("Anthony", "anthony123");
        waiterAvailability.AddName("Maboyi", "maboyi123");
        waiterAvailability.AddName("Skantsotso", "skantsotso123");

        var expected = connection.Query<Waiter>(@"SELECT * FROM waiters").ToList().Count;
        // When
        var actual = waiterAvailability.GetWaiters().Count;
        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldBeAbleToAddWaiterSelectedDaysToSchedule()
    {
        // Given
        var waiterAvailability = new WaiterAvailability(GetConnectionString());
        DropTables();
        CreateTables();
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        waiterAvailability.AddName("Anthony", "anthony123");

        waiterAvailability.AddToScheduleWithDates("Anthony", new List<string>() { "02/03/23", "03/03/23" });
        var expected = connection.Query<Schedule>(@"
                    SELECT w.firstname, s.dates FROM schedule s
                    INNER JOIN waiters w ON w.id = s.waiter_id").ToList();
        // When
        var actual = waiterAvailability.GetSchedule();
        // Then
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ShouldBeAbleToReturnTheNumberOfWeekingDays()
    {
        // Given
        var waiterAvailability = new WaiterAvailability(GetConnectionString());
        DropTables();
        CreateTables();
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        var expected = connection.Query<Weekday>(@"SELECT * FROM weekdays").ToList().Count;

        // When
        var actual = waiterAvailability.GetWeekdays().ToList().Count;
        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldBeAbleToGetDaysWaiterSelected()
    {
        // Given
        var waiterAvailability = new WaiterAvailability(GetConnectionString());
        DropTables();
        CreateTables();
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        waiterAvailability.AddName("John", "mog123");

        waiterAvailability.AddToScheduleWithDates("John", new List<string>() { "02/03/23", "03/03/23" });
        var expected = connection.Query<Schedule>(@"
             SELECT s.id, w.firstname, s.dates FROM schedule s
             INNER JOIN waiters w ON w.id = s.waiter_id
             WHERE w.firstname = 'John'").ToList();

        // When
        var actual = waiterAvailability.WaiterWorkingDates("John");
        // Then
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }
}