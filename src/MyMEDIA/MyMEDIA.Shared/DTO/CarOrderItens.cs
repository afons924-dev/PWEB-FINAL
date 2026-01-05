namespace MyMEDIA.Shared.DTO;

public class CarOrderItens
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
}
