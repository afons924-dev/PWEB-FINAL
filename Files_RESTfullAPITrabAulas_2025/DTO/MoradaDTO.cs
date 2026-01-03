using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulAPIWeb.DTO;

public class MoradaDTO
{
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
}
