using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Products;

[Table("product", Schema = "bobsusedbookstore_dbo")]
public class Product
{
    [Key]
    [Column("productid")]
    public int ProductID { get; set; }

    [Required]
    [StringLength(15)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    [Column("productnumber")]
    public string ProductNumber { get; set; } = string.Empty;

    [Required]
    [Column("safetystocklevel")]
    public int SafetyStockLevel { get; set; }
}