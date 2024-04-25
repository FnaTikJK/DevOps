using DevOpsAPI.DAL;
using DevOpsAPI.Infra;
using DevOpsAPI.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevOpsAPI.Messages;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IQueryable<MessageEntity> messages;
    private readonly DataContext db;

    public MessagesController(DataContext db)
    {
        this.db = db;
        messages = db.Messages.Include(m => m.Author).Include(m => m.Files);
    }

    [HttpGet("{messageId:Guid}")]
    public async Task<ActionResult<MessageEntity>> GetMessage([FromRoute] Guid messageId)
    {
        var message = await messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
            return BadRequest("Message doesn't exist");

        return Ok(message);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageEntity>>> GetMessages()
    {
        return await messages.ToArrayAsync();
    }

    [HttpPost]
    public async Task<ActionResult<PostResponse<MessageEntity>>> CreateOrUpdate(MessageCreateOrUpdateReq request)
    {
        var user = await db.Accounts.FindAsync(User.GetId());
        if (user == null)
            return BadRequest("Пользователя не существует");

        MessageEntity? message;
        if (request.Id != null)
        {
            message = await messages.FirstOrDefaultAsync(m => m.Id == request.Id);
            if (message == null)
                return BadRequest("Message doesn't exist");
            
            message.IsEdited = true;
        }
        else
        {
            message = new MessageEntity
            {
                Author = user,
                DateTime = DateTime.Now,
                Text = request.Text ?? "",
                Files = new HashSet<FileEntity>(),
            };
            await db.Messages.AddAsync(message);
        }
        
        if (request.Text != null)
            message.Text = request.Text;
        if (request.FileIds != null)
        {
            if (!TryResolveFiles(request.FileIds, out var files))
                return BadRequest("Incorrect file ids");
            message.Files = files;
        }

        await db.SaveChangesAsync();

        return new PostResponse<MessageEntity>
        {
            Item = message,
            IsCreated = request.Id == null,
        };
    }

    [HttpDelete("{messageId:Guid}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid messageId)
    {
        var existed = await db.Messages.FindAsync(messageId);
        if (existed != null)
        {
            db.Messages.Remove(existed);
            await db.SaveChangesAsync();
        }

        return NoContent();
    }

    private bool TryResolveFiles(HashSet<Guid> fileIds, out HashSet<FileEntity> files)
    {
        files = new HashSet<FileEntity>();
        foreach (var fileId in fileIds)
        {
            var file = db.Files.Find(fileId);
            if (file == null)
                return false;
            
            files.Add(file);
        }

        return true;
    }
}