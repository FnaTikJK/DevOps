using Amazon.Runtime;
using Amazon.SQS;
using DevOpsAPI.Infra;

namespace DevOpsAPI;

public interface IQueueService
{
    Task SendMessage(string message);
}

public class QueueService : IQueueService
{
    private readonly string queueUrl;
    private readonly AmazonSQSClient client;
    
    public QueueService(BasicAWSCredentials creds)
    {
        var amazonConfig = new AmazonSQSConfig()
        {
            
        };
        
        client = new AmazonSQSClient(creds);
        queueUrl = Config.Yandex.QueueUrl;
    }


    public async Task SendMessage(string message)
    {
        await client.SendMessageAsync(queueUrl, message);
    }
}