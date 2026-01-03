namespace MyMEDIA.Shared.Entities;

public class Favorite
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
