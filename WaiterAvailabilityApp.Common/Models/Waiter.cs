using System.ComponentModel.DataAnnotations;

namespace WaiterAvailabilityApp.Model;
public class Waiter
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Login name is required!")]
    public string? FirstName { get; set; }
}