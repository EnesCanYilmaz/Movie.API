namespace MovieAPI.Infrastructure.Data;

internal static class ServiceRegistration
{
    internal static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<MovieAPIDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<MovieAPIDbContext>();
    }

    internal static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenHandler, TokenHandler>();

        return services;
    }

    internal static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = "https://api.movieapp.com.tr",
                ValidIssuer = "https://angular.movieapp.com.tr",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MovieApplicationInformation")),
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
                NameClaimType = ClaimTypes.Name
            };
        });
        
        return services;
    }
}