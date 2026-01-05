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

    // Alias/Mapped properties for Client compatibility
    public string Nome { get => Title; set => Title = value; }
    public decimal Preco { get => FinalPrice; set => FinalPrice = value; }
    public string Origem { get => Origin; set => Origin = value; }
    public bool Favorito { get; set; } // Client-side specific
    public string UrlImagem { get => ImageUrl; set => ImageUrl = value; }
    public byte[]? Imagem { get => Image; set => Image = value; }
    public string Detalhe { get => Description; set => Description = value; }
    public int EmStock { get => StockQuantity; set => StockQuantity = value; }
    public bool Disponivel { get => IsForSale && IsActive && StockQuantity > 0; set { /* readonly derived */ } }

    // Potentially missing fields or complex types
    public DeliveryModeDTO modoentrega { get; set; } = new(); // Specific to client logic
}
