using System.Security.Cryptography.X509Certificates;
using DevOpsAPI.Accounts;
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
        options.UseNpgsql(
            Config.DbConnection,
            builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                if (Config.Yandex.PathToCert != null)
                {
                    builder.ProvideClientCertificatesCallback(clientCerts =>
                    {
                        var clientCertPath = Config.Yandex.PathToCert;
                        // To avoid permission ex run: "sudo chmod -R 777 /home/username/.postgresql/root.crt"
                        var cert = new X509Certificate2(clientCertPath);
                        clientCerts.Add(cert);
                    });
                }
            }
        );
    }

    public void CreateDatabase()
    {
        Database.EnsureCreated();
    }

    public void DeleteDatabase()
    {
        Database.EnsureDeleted();
    }

    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
}