namespace DevOpsAPI.Statics;

public interface IStaticsService
{
    Task Create(string key, Stream stream);
    Task Delete(string key);
}