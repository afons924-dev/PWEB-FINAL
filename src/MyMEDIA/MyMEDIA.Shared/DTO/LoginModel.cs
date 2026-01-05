namespace MyMEDIA.Shared.DTO;

public class LoginModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Case-insensitive aliases
    public string email { get => Email; set => Email = value; }
    public string password { get => Password; set => Password = value; }
}
