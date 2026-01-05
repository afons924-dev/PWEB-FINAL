namespace MyMEDIA.Shared.DTO;

public class DeliveryModeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Alias for Client
    public string Nome { get => Name; set => Name = value; }
}
