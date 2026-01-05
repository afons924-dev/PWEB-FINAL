namespace MyMEDIA.Shared.DTO;

public class RegisterModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserType { get; set; } = "Client";
    public string? NIF { get; set; }
    public string Address { get; set; } = string.Empty;

    public string? Nome { get; set; }
    public string? Apelido { get; set; }
    public string? Telemovel { get; set; }
    public string? Rua { get; set; }
    public string? Localidade { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Cidade { get; set; }
    public string? Pais { get; set; }
}
