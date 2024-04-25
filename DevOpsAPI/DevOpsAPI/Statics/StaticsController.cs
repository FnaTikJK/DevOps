using System.Text.Json;
using DevOpsAPI.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Physical;

namespace DevOpsAPI.Statics;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StaticsController : ControllerBase
{
    private readonly IStaticsService staticsService;
    private readonly DataContext db;

    public StaticsController(IStaticsService staticsService, DataContext db)
    {
        this.staticsService = staticsService;
        this.db = db;
    }

    [HttpPost("Upload")]
    public async Task<ActionResult> Upload(IFormFile file)
    {
        var key = Guid.NewGuid().ToString();
        await using(var stream = file.OpenReadStream()){
            await staticsService.Create(key, stream);
        }
        
        var fileEntity = new FileEntity
        {
            Key = key,
            FileName = file.FileName,
            ContentType = file.ContentType,
        };
        await db.Files.AddAsync(fileEntity);
        await db.SaveChangesAsync();

        return Ok(new {FileId = fileEntity.Id});
    }

    [HttpPost("Download")]
    public async Task<FileStreamResult> Download([FromBody] DownloadFileReq request)
    {
        var existed = await db.Files.FindAsync(request.FileId);
        if (existed == null)
            throw new Exception("File doesn't existed");
        var stream = await staticsService.Get(existed.Key);
        return new FileStreamResult(stream, existed.ContentType)
        {
            FileDownloadName = existed.FileName,
        };

        //await HttpContext.Response.SendFileAsync(fileInfo);
    }

    public class DownloadFileReq
    {
        public Guid FileId { get; set; }
    }
}