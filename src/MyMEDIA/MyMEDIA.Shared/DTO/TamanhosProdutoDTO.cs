namespace MyMEDIA.Shared.DTO;

public class TamanhosProdutoDTO
{
    public string Tamanho { get; set; } = string.Empty;
    public string TamanhoItem { get; set; } = string.Empty; // Alias or specific field
    public decimal Preco { get; set; }
}
