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

    public class UploadReq
    {
        public IFormFile[] Files { get; set; }
    }

    [Consumes("multipart/form-data")]
    [HttpPost("Upload")]
    public async Task<ActionResult> Upload([FromForm]UploadReq req)
    {
        if (req.Files.Length == 0)
            return BadRequest("Empty Files");
        
        var files = req.Files;
        var fileEntities = new List<FileEntity>(files.Length);
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var key = Guid.NewGuid().ToString();
            await staticsService.Create(key, stream);
            fileEntities.Add(new FileEntity
            {
                Key = key,
                FileName = file.FileName,
                ContentType = file.ContentType,
            });
        }
        
        await db.Files.AddRangeAsync(fileEntities);
        await db.SaveChangesAsync();

        return Ok(new {FileIds = fileEntities.Select(f => f.Id)});
    }

    [HttpPost("Download/{fileId:Guid}")]
    public async Task<FileStreamResult> Download([FromRoute] Guid fileId)
    {
        var existed = await db.Files.FindAsync(fileId);
        if (existed == null)
            throw new Exception("File doesn't existed");
        var stream = await staticsService.Get(existed.Key);
        return new FileStreamResult(stream, existed.ContentType)
        {
            FileDownloadName = existed.FileName,
        };

        //await HttpContext.Response.SendFileAsync(fileInfo);
    }
}