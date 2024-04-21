namespace DevOpsAPI.Infra;

public class Config
{
    public const string JwtSecurityKey = "Token with 16 ch";
    
    
    public static string GetDbConnection(IConfiguration configuration)
    {
        // Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
        return configuration.GetConnectionString("DefaultConnection");
    }
}