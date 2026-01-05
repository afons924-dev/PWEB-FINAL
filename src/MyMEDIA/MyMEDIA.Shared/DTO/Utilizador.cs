using System.ComponentModel.DataAnnotations;
using MyMEDIA.Shared.Validations;

namespace MyMEDIA.Shared.DTO;

public class Utilizador
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apelido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [validarNIF(ErrorMessage = "O NIF é inválido.")]
    public long? NIF { get; set; }

    [RegularExpression(@"^9\d{8}$", ErrorMessage = "O número de telemóvel deve ter 9 dígitos e começar com '9'.")]
    public string Telemovel { get; set; } = string.Empty;

    [Required]
    public string Rua { get; set; } = string.Empty;

    [Required]
    public string Localidade { get; set; } = string.Empty;

    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato '1234-567'.")]
    public string CodigoPostal { get; set; } = string.Empty;

    [Required]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    public string Pais { get; set; } = string.Empty;
}
