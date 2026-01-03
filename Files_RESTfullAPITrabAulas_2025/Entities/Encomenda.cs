using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RestfulAPIWeb.Data;

namespace RestfulAPIWeb.Entities;
public class Encomenda
{
	public int Id { get; set; }
	public DateTime Data { get; set; } 
	public EstadoEncomenda Estado { get; set; }
	public Boolean pagamentoEfetuado { get; set; }
    public Boolean pagamentoConfirmadoInternamente { get; set; }

    // Dados Entrega
    [Required]
    [StringLength(100)]
    public string? Rua { get; set; }
    [Required]
    [StringLength(100)]
    public string? Localidade1 { get; set; }
    [Required]
    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato '1234-567'.")]
    public string? CodigoPostal { get; set; }
    [Required]
    [StringLength(100)]
    public string? Cidade { get; set; }
    [Required]
    [StringLength(100)]
    public string? Pais { get; set; }

    // Atributos de Relacionamento com outras Entidades //

    // Relacionamento com Cliente
    [JsonIgnore]
    public string ClienteId { get; set; } = null!;
    [JsonIgnore]
    public ApplicationUser Cliente { get; set; } = null!;

	// Relacionamento com EncomendaItem (Produtos Encomendados)
	public ICollection<EncomendaItem> ProdutosEncomendados { get; set; } = new List<EncomendaItem>();
	
}

// AUXILIAR ESTADO ENCOMENDA
public enum EstadoEncomenda
{
	Pendente,
	Enviada,
	Cancelada
}

public static class EstadoEncomendaAuxiliares
{
	public static string EstadoEntrega(this EstadoEncomenda estado)
	{
		return estado switch
		{
			EstadoEncomenda.Pendente => "Pendente",
			EstadoEncomenda.Enviada => "Enviada",
			EstadoEncomenda.Cancelada => "Cancelada",
			_ => "Estado Desconhecido"
		};
	}
}
