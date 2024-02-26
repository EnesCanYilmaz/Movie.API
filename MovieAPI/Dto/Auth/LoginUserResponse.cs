namespace MovieAPI.Dto.Auth;

public class LoginUserResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public AppUser? User { get; set; }
}