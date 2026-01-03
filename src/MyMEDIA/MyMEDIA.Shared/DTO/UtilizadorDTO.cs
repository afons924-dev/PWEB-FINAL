using MyMEDIA.Shared.Data;
using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Shared.DTO;
public class UtilizadorDTO
{
    public string Email { get; set; }
    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(100)]
    public string? Apelido { get; set; }
    [Required]
    [StringLength(100)]
    public string? Rua { get; set; }
    [Required]
    [StringLength(100)]
    public string? Localidade { get; set; }

    [Required]
    [StringLength(100)]
    public string? Pais { get; set; }
    public string Estado { get; set; }
    [Required]
    [StringLength(100)]
    public string? Cidade { get; set; }
    public DateTime DataRegisto { get; set; }

    [RegularExpression(@"^9\d{8}$", ErrorMessage = "O número de telemóvel deve ter 9 dígitos e começar com '9'.")]
    public string? Telemovel { get; set; }
    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato '1234-567'.")]
    public string? CodigoPostal { get; set; }

    [validarNIF(ErrorMessage = "O NIF é inválido.")]
    public long? NIF { get; set; }
}
