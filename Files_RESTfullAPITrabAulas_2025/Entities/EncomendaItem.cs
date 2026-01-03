using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestfulAPIWeb.Entities;

public class EncomendaItem
{
	public int Id { get; set; }
    [Required]
	[Range(0, Double.MaxValue, ErrorMessage = "A Quantidade Encomendada não pode ser negativa")]
	[Column(TypeName = "decimal(10,2)")]
    public decimal Quantidade { get; set; }

    // Nota Importante: Optou-se por guardar alguns atributos de Produto e Categoria no Momento da Encomenda,
    // dado que podem ser alterados posteriormente e não queremos que a informação seja alterada na Encomenda

    // Relacionamento com Encomenda
    public int EncomendaId { get; set; }
    [JsonIgnore]
    public Encomenda Encomenda { get; set; } = null!;

    // Atributos do Produto no Momento da Encomenda
    [JsonIgnore]
    public int ProdutoId { get; set; }
	public Produto Produto { get; set; } = null!;
	[StringLength(100)]
	public string NomeProduto { get; set; } = "";
    [Required]
	[Range(0, Double.MaxValue, ErrorMessage = "O Preço não pode ser negativo")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
	[StringLength(100)]
	public string DetalheProduto { get; set; } = "";

	// Atributos do Modo de Disponibilização do Produto no Momento da Encomenda
	[StringLength(100)]
	public string ModoEntrega { get; set; } = "";
	[StringLength(100)]
	public string? DetalheModoDisponibilizacao { get; set; }

	// Atributos da Categoria no Momento da Encomenda
	[StringLength(100)]
	public string NomeCategoria { get; set; } = "";

}
