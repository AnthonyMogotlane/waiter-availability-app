using System.Data.Common;
using System.Text.Json;
using Dapper;
using Npgsql;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Data;
using WaiterAvailabilityApp.Model;
using Xunit;

namespace WaiterAvailabilityApp.Tests;

public class WaiterAvailabilityTest
{
    static string cs = "Host=localhost;Username=postgres;Password=0000;Database=test_db";
    static string GetConnectionString()
    {
        var csEnv = Environment.GetEnvironmentVariable("PSQLConnectionString");
        if(csEnv == null) csEnv = cs;
        return csEnv;
    }

    public void DropTables()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            connection.Execute(File.ReadAllText("../../../../sql/dropTables.sql"));
        }
    }

    public void CreateTables()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            connection.Execute(File.ReadAllText("../../../../sql/tables.sql"));
        }
    }

    public void PopulateWeekDays()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            connection.Execute(File.ReadAllText("../../../../sql/insertWeekdays.sql"));
        }
    }

    [Fact]
    public void ShouldBeAbleToAddWaitersName()
    {
        // Given
        var applicationDB = new ApplicationDB(cs);
        DropTables();
        CreateTables();
        PopulateWeekDays();
        using var connection = new NpgsqlConnection(cs);
        connection.Open();

        applicationDB.AddName("Fabiano");
        var expected = connection.Query<Waiter>(@"SELECT * FROM waiters").ToList();

        // When 
        var actual = applicationDB.GetWaiters();

        // Then 
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ShouldBeAbleToReturnTheCountOfWaitersInWaitersTable()
    {
        // Given
        var applicationDB = new ApplicationDB(cs);
        DropTables();
        CreateTables();
        PopulateWeekDays();
        using var connection = new NpgsqlConnection(cs);
        connection.Open();

        applicationDB.AddName("Anthony");
        applicationDB.AddName("Maboyi");
        applicationDB.AddName("Skantsotso");

        var expected = connection.Query<Waiter>(@"SELECT * FROM waiters").ToList().Count;
        // When
        var actual = applicationDB.GetWaiters().Count;
        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldBeAbleToAddWaiterSelectedDaysToSchedule()
    {
        // Given
        var applicationDB = new ApplicationDB(cs);
        DropTables();
        CreateTables();
        PopulateWeekDays();
        using var connection = new NpgsqlConnection(cs);
        connection.Open();

        applicationDB.AddName("Anthony");

        applicationDB.AddToSchedule("Anthony", new List<int>() { 1, 2, 3 });
        var expected = connection.Query<Schedule>(@" 
                    SELECT w.firstname, wd.day FROM weekdays wd
                    INNER JOIN schedule s ON wd.id = s.day_id
                    INNER JOIN waiters w ON w.id = s.waiter_id").ToList();
        // When
        var actual = applicationDB.GetSchedule();
        // Then
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ShouldBeAbleToReturnTheNumberOfWeekingDays()
    {
        // Given
        var applicationDB = new ApplicationDB(cs);
        DropTables();
        CreateTables();
        PopulateWeekDays();
        using var connection = new NpgsqlConnection(cs);
        connection.Open();

        var expected = connection.Query<Weekday>(@"SELECT * FROM weekdays").ToList().Count;

        // When
        var actual = applicationDB.GetWeekdays().ToList().Count;
        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldBeAbleToGetDaysWaiterSelected()
    {
        // Given
        var applicationDB = new ApplicationDB(cs);
        DropTables();
        CreateTables();
        PopulateWeekDays();
        using var connection = new NpgsqlConnection(cs);
        connection.Open();

        applicationDB.AddName("Anthony");

        applicationDB.AddToSchedule("Anthony", new List<int>() { 1, 2, 3 });

        var expected = connection.Query<Weekday>(@" 
                    SELECT wd.id, wd.day, w.firstname FROM weekdays wd
                    INNER JOIN schedule s ON wd.id = s.day_id
                    INNER JOIN waiters w ON w.id = s.waiter_id
                    WHERE w.firstname = 'Anthony'");

        // When
        var actual = applicationDB.WaiterWorkingDays("Anthony");
        // Then
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }
}