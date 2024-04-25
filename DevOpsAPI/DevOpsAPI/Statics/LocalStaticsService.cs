using DevOpsAPI.Infra;

namespace DevOpsAPI.Statics;

public class LocalStaticsService : IStaticsService
{
    private readonly string pathToStatic;

    public LocalStaticsService()
    {
        this.pathToStatic = Config.Local.PathToStatic;
    }

    public async Task Create(string key, Stream stream)
    {
        await using var fileStream = File.Create(pathToStatic + "/" + key);
        await stream.CopyToAsync(fileStream);
    }

    public async Task<Stream> Get(string key)
    {
        var path = pathToStatic + "/" + key;
        if (!File.Exists(path))
            throw new ArgumentException("File doesn't exist in file system");
        var stream = File.Open(path, FileMode.Open);

        return stream;
    }

    public Task Delete(string key)
    {
        throw new NotImplementedException();
    }
}