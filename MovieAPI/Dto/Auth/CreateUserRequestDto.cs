namespace MovieAPI.Dto.Auth;

public sealed record CreateUserModel
{
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
}