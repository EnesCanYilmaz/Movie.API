namespace MovieAPI.Dto.Auth;

public sealed record CreateUserResponse()
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
}