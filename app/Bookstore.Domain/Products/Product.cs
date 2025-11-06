using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Products;

[Table("product", Schema = "public")]
public class Product
{
    [Key]
    [Column("product_id")]
    public int ProductID { get; set; }

    [Required]
    [StringLength(15)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    [Column("product_number")]
    public string ProductNumber { get; set; }

    [Required]
    [Column("safety_stock_level")]
    public int SafetyStockLevel { get; set; }
}