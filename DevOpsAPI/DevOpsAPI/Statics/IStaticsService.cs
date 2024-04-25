namespace DevOpsAPI.Statics;

public interface IStaticsService
{
    Task Create(string key, Stream stream);
    Task<Stream> Get(string key);
    Task Delete(string key);
}