using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Shared.Entities;

public class DeliveryMode
{
    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
}
