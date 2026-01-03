using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Shared.Entities;

public class ShoppingCartItem
{
    public int Id { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string ClientId { get; set; } = string.Empty;
}
