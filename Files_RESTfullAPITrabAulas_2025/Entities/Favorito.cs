using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

using RestfulAPIWeb.Data;

namespace RestfulAPIWeb.Entities;

public class Favorito
{
	public int Id { get; set; }
	public string ClienteId { get; set; } = null!;
    //[JsonIgnore]
    //public ApplicationUser Cliente { get; set; } = null!;
	public int ProdutoId { get; set; }
    [JsonIgnore]
    public Produto Produto { get; set; } = null!;
}
