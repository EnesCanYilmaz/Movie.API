namespace MovieAPI.Infrastructure.Data.Context;

public class MovieAPIDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public MovieAPIDbContext(DbContextOptions options) : base(options)
    {

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker.Entries<BaseEntity>();

        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.Now,
                EntityState.Modified => data.Entity.UpdatedDate = DateTime.Now,
                _ => DateTime.Now
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Movie>().HasMany(m => m.MovieImages).WithOne(p => p.Movie).HasForeignKey(p => p.MovieId);

        base.OnModelCreating(builder);
        base.OnModelCreating(builder);
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieImage> MovieImages { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Player> Players { get; set; }

}

