namespace DevOpsAPI.Infra;

public class Config
{
    public static string JwtSecurityKey = string.Concat(Enumerable.Repeat("Token with 16 ch", 5).ToArray());


    public static string DbConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION") 
                                        ?? "Server=localhost;Port=5432;Database=DevOps;User Id=postgres;Password=password";


    public static YandexSettings Yandex = new()
    {
        UseCloud = ParseEnvVar("YANDEX_USE_CLOUD"),
        PathToCert = Environment.GetEnvironmentVariable("YANDEX_PATH_TO_CERT")!,
        AccountAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!,
        AccountSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY")!,
        BucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME")!,
    };




    private static bool ParseEnvVar(string envName)
    {
        var envVar = Environment.GetEnvironmentVariable(envName);
        if (envVar == null)
            return false;

        if (!Boolean.TryParse(envVar, out var res))
            throw new ArgumentException($"Incorrect boolean var: {envName}");

        return res;
    }
}

public class YandexSettings
{
    public bool UseCloud { get; set; }
    public string PathToCert { get; set; }
    public string AccountAccessKey { get; set; }
    public string AccountSecretKey { get; set; }
    public string BucketName { get; set; }
    public string QueueUrl { get; set; }
}