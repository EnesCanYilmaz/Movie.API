namespace MovieAPI.Dto.Token;

public record Token()
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; }
}