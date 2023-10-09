namespace MovieAPI.Infrastructure.Data;

static class Configuration
{
    static public string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();

            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString("MSSQL") ?? "DefaultConnectionString";
        }
    }
}

