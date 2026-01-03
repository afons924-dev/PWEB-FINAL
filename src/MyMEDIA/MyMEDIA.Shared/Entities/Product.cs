using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMEDIA.Shared.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public byte[]? Image { get; set; }

    public string Origin { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalPrice { get; set; }

    public int StockQuantity { get; set; } = 0;

    public bool IsActive { get; set; } = false; // Requires approval (Pending -> Active)
    public bool IsForSale { get; set; } = true; // vs Listing only

    public bool IsPromotion { get; set; } = false;
    public bool IsBestSeller { get; set; } = false;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int? DeliveryModeId { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }

    public string SupplierId { get; set; } = string.Empty; // User ID of supplier
}
