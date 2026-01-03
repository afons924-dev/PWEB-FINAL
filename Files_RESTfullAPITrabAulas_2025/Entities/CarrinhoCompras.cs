using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

using RestfulAPIWeb.Data;
using System.ComponentModel.DataAnnotations;

namespace RestfulAPIWeb.Entities;

public class CarrinhoCompras
{
	public int Id { get; set; }
	[Range(0, Double.MaxValue, ErrorMessage = "A Quantidade não pode ser negativa")]
	public double Quantidade { get; set; }

	// Atributos de Relacionamento com outras Entidades //

	// Relacionamento com Produto
	public int ProdutoId { get; set; }
    [JsonIgnore]
    public Produto Produto { get; set; } = null!;

	// Relacionamento com Cliente
	public string ClienteId { get; set; } = null!;
	//[JsonIgnore]
	//public ApplicationUser Cliente { get; set; } = null!;

}
