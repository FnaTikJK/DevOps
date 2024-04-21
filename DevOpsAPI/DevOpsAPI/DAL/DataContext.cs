using DevOpsAPI.Infra;
using Microsoft.EntityFrameworkCore;

namespace DevOpsAPI.DAL;

public class DataContext : DbContext
{
    private readonly IConfiguration config;

    public DataContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        this.config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        options.UseNpgsql(Config.GetDbConnection(config),
            builder => { builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); });
    }

    public void CreateDatabase()
    {
        Database.EnsureCreated();
    }

    public void DeleteDatabase()
    {
        Database.EnsureDeleted();
    }
}