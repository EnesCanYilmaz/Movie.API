namespace MovieAPI.Infrastructure.Data;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
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
}