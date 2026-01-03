namespace MyMEDIA.Shared.DTO;

public class ProdutoFavorito
{
    public int ProdutoId { get; set; }
    public string? Nome { get; set; }
    public string? Imagem { get; set; }
    public decimal Preco { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ProdutoFavorito other)
        {
            return ProdutoId == other.ProdutoId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return ProdutoId.GetHashCode();
    }
}
