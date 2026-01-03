using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyMEDIA.Shared.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    [ForeignKey("ParentId")]
    [JsonIgnore] // Avoid cycles in serialization
    public Category? Parent { get; set; }

    public List<Category> SubCategories { get; set; } = new();

    // Navigation property
    [JsonIgnore] // Avoid cycles
    public List<Product> Products { get; set; } = new();
}
