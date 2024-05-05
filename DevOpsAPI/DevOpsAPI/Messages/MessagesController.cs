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
    private readonly MessagesNotifier notifier;

    public MessagesController(DataContext db, MessagesNotifier notifier)
    {
        this.db = db;
        this.notifier = notifier;
        messages = db.Messages.Include(m => m.Author).Include(m => m.Files).AsQueryable();
    }

    [HttpGet("{messageId:Guid}")]
    public async Task<ActionResult<MessageOut>> GetMessage([FromRoute] Guid messageId)
    {
        var message = await messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
            return BadRequest("Message doesn't exist");

        return Ok(Map(message));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageOut>>> GetMessages()
    {
        return messages.OrderBy(m => m.DateTime).Select(Map).ToArray();
    }

    [HttpPost]
    public async Task<ActionResult<PostResponse<MessageOut>>> CreateOrUpdate(MessageCreateOrUpdateReq request)
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
        await notifier.Notify(message.Id, MessagesNotifier.NotifyType.CreateOrUpdate);

        return new PostResponse<MessageOut>
        {
            Item = Map(message),
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
            await notifier.Notify(messageId, MessagesNotifier.NotifyType.Delete);
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

    private MessageOut Map(MessageEntity source) => new MessageOut
    {
        Id = source.Id,
        DateTime = source.DateTime,
        IsEdited = source.IsEdited,
        Text = source.Text,
        Author = new AuthorOut
        {
            Id = source.Author.Id,
            Login = source.Author.Login,
        },
        Files =
            source.Files.Select(f => new FileOut()
            {
                Id = f.Id,
                contentType = f.ContentType,
                fileName = f.FileName,
                Key = f.Key,
            }).ToArray()
    };
    
    public class MessageOut
    {
        public Guid Id { get; set; }
        public AuthorOut Author { get; set; }
        public FileOut[] Files { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsEdited { get; set; }
    }

    public class AuthorOut
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
    }

    public class FileOut
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
    }
}