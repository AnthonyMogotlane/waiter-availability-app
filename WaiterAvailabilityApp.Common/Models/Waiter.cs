using System.ComponentModel.DataAnnotations;

namespace WaiterAvailabilityApp.Model;
public class Waiter
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required!")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Password is required!")]
    public string? Password { get; set; }
}