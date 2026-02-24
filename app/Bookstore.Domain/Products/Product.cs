using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain.Products;

public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    [StringLength(15)]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    public string ProductNumber { get; set; }

    [Required]
    public int SafetyStockLevel { get; set; }
}