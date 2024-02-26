namespace MovieAPI.Application.Services.Token;

public interface ITokenHandler
{
    Dto.Token.Token CreateAccessToken(int second, AppUser? appUser);
    string CreateRefreshToken();
}