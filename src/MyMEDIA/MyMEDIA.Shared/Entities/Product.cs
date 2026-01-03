using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMEDIA.Shared.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalPrice { get; set; }

    public int StockQuantity { get; set; } = 0;

    public bool IsActive { get; set; } = false; // Requires approval
    public bool IsForSale { get; set; } = true; // vs Listing only

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string SupplierId { get; set; } = string.Empty; // User ID of supplier
}
