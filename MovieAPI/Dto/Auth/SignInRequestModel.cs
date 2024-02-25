namespace MovieAPI.Dto.Auth;

public record SignInRequestModel()
{
    public string UserNameOrEmail { get; init; }
    public string Password { get; init; }
}