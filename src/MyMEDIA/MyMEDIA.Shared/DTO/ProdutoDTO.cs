namespace MyMEDIA.Shared.DTO;

public class ProdutoDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
    public string Origin { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public bool IsForSale { get; set; }
    public bool IsPromotion { get; set; }
    public bool IsBestSeller { get; set; }
    public int CategoryId { get; set; }
    // Add other fields as necessary from Product entity
}
