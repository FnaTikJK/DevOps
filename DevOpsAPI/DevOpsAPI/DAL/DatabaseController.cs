using System.Text.Json;
using DevOpsAPI.Infra;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsAPI.DAL;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly DataContext db;

    public DatabaseController(DataContext db)
    {
        this.db = db;
    }

    [HttpGet("Info")]
    public ActionResult GetInfo()
    {
        return Ok(new
        {
            UseCloud = Config.Yandex.UseCloud,
            PathToCert = Config.Yandex.PathToCert,
            AccountAccessKey = Config.Yandex.AccountAccessKey,
            AccountSecretKey = Config.Yandex.AccountSecretKey,
            BucketName = Config.Yandex.BucketName,
            DbConnection = Config.DbConnection,
        });
    }

    [HttpPost]
    public ActionResult CreateDatabase()
    {
        db.CreateDatabase();
        return NoContent();
    }
    
    [HttpPost("Full")]
    public ActionResult RecreateDatabase()
    {
        try
        {
            db.DeleteDatabase();
        }
        catch
        {
            // ignored
        }

        db.CreateDatabase();
        return NoContent();
    }

    [HttpDelete]
    public ActionResult DeleteDatabase()
    {
        db.DeleteDatabase();
        return NoContent();
    }
}