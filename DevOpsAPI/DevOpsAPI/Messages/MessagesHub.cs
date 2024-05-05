using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevOpsAPI.Messages;

public class MessagesNotifier
{
     private readonly IHubContext<MessagesHub> context;

     public MessagesNotifier(IHubContext<MessagesHub> context)
     {
          this.context = context;
     }

     public async Task Notify(Guid messageId, NotifyType type)
     {
          await context.Clients.All.SendAsync("Notify", new Notification{MessageId = messageId, Type = type});
     }

     public class Notification
     {
          public Guid MessageId { get; set; }
          public NotifyType Type { get; set; }
     }

     public enum NotifyType
     {
          CreateOrUpdate,
          Delete,
     }
}

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MessagesHub : Hub
{
    
}