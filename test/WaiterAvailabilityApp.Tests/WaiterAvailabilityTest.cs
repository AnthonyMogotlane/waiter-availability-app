using System.Data.Common;
using System.Text.Json;
using Dapper;
using Npgsql;
using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Data;
using WaiterAvailabilityApp.Model;
using XUnit;

namespace WaiterAvailabilityApp.Tests;

public class WaiterAvailabilityTest
{
    string cs = "";

    public void DropTables()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            connection.Execute(@"
                DROP TABLE IF EXISTS schedule;
                DROP TABLE IF EXISTS waiters;
                DROP TABLE IF EXISTS weekdays;");
        }
    }

    public void CreateTables()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            var query = System.IO.File.ReadAllText("");
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS weekdays (
                id serial PRIMARY KEY,
                day VARCHAR(50) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS waiters (
                id serial PRIMARY KEY,
                firstname VARCHAR(50) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS schedule (
                id serial PRIMARY KEY,
                day_id int NOT NULL,
                waiter_id int NOT NULL,
                FOREIGN KEY (day_id) REFERENCES weekdays(id),
                FOREIGN KEY (waiter_id) REFERENCES waiters(id)
            );");
        }
    }

    public void PopulateWeekDays()
    {
        using (var connection = new NpgsqlConnection(cs))
        {
            connection.Execute(@"
                INSERT INTO weekdays (day) VALUES 
                ('Monday'),
                ('Tuesday'),
                ('Wednesday'),
                ('Thursday'),
                ('Friday'),
                ('Saturday'),
                ('Sunday');
            ");
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
        
        applicationDB.AddToSchedule("Anthony", new List<int>(){1, 2, 3});
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

        applicationDB.AddToSchedule("Anthony", new List<int>(){1, 2, 3});
        
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