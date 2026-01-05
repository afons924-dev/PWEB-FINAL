namespace MyMEDIA.Shared.DTO;

public class Token
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string UserType { get; set; } = string.Empty;
    public string UtilizadorNome { get; set; } = string.Empty;
}
