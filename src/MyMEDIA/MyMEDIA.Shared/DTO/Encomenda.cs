namespace MyMEDIA.Shared.DTO;

public class Encomenda
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    // Add other fields as necessary
}
