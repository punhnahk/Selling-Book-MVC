using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1670.Models;

public class Orders
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public DateTime Order_Date { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public double Total { get; set; }
   
    //link to User
    [ForeignKey("UserId")]
    public User Users { get; set; }
}