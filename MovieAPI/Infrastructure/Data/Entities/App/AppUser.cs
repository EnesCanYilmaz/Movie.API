namespace MovieAPI.Infrastructure.Data.Entities.App;

public class AppUser : IdentityUser<int>
{
    public string? FullName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}

