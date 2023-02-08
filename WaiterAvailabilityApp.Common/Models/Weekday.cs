using System.ComponentModel.DataAnnotations;

namespace WaiterAvailabilityApp.Model;
public class Weekday
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Day { get; set; }
}