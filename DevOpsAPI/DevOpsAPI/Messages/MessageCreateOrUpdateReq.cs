namespace DevOpsAPI.Messages;

public class MessageCreateOrUpdateReq
{
    public Guid? Id { get; set; }
    public string? Text { get; set; }
    public HashSet<Guid>? FileIds { get; set; }
}