namespace MovieAPI.Application.Services.Token;

public class TokenHandler : ITokenHandler
{
    public Dto.Token.Token CreateAccessToken(int second, AppUser? appUser)
    {
        Dto.Token.Token token = new();

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("MovieInformationApplicationWebAPI"));

        token.Expiration = DateTime.UtcNow.AddSeconds(second);
        JwtSecurityToken securityToken = new(audience: "https://api.movieapp.com.tr", issuer: "https://angular.movieapp.com.tr", expires: token.Expiration, notBefore: DateTime.UtcNow, signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256), claims: new List<Claim> { new(ClaimTypes.Name, appUser.UserName ?? throw new InvalidOperationException("Kullanıcı bulunamadı")) });

        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);

        token.RefreshToken = CreateRefreshToken();
        return token;
    }

    public string CreateRefreshToken()
    {
        var number = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }
}