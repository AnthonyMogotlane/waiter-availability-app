using System.ComponentModel.DataAnnotations;

namespace WaiterAvailabilityApp.Model;
public class Schedule
{
    public int Id { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? Day { get; set; }
    [Required]
    public string? Dates { get; set; }
}