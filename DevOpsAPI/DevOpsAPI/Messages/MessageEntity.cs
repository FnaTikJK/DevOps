using System.ComponentModel.DataAnnotations;
using DevOpsAPI.Accounts;
using DevOpsAPI.Statics;

namespace DevOpsAPI.Messages;

public class MessageEntity
{
    [Key]
    public Guid Id { get; set; }
    public AccountEntity Author { get; set; }
    public HashSet<FileEntity> Files { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsEdited { get; set; }
}