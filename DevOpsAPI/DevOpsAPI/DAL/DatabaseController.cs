using Microsoft.AspNetCore.Authorization;
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