using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DevOpsAPI.Infra;

namespace DevOpsAPI.Statics;

public class StaticsService : IStaticsService
{
    private readonly string bucketName;
    private readonly AmazonS3Client client;
    
    public StaticsService(BasicAWSCredentials creds)
    {
        var amazonConfig = new AmazonS3Config
        {
            ServiceURL = "https://s3.yandexcloud.net",
        };
        
        client = new AmazonS3Client(creds, amazonConfig);
        bucketName = Config.Yandex.BucketName;
    }

    public async Task Create(string key, Stream stream)
    {
        var req = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = stream
        };

        await client.PutObjectAsync(req);
    }
    
    public async Task Delete(string key)
    {
        await client.DeleteObjectAsync(bucketName, key);
    }
}