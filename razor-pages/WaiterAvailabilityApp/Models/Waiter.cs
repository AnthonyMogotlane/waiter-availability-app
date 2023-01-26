using System.ComponentModel.DataAnnotations;

namespace WaiterAvailabilityAppRazor.Models;
public class Waiter
{
    public int Id { get; set; }
    [Required]
    public string? FirstName { get; set; }
}