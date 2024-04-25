using System.ComponentModel.DataAnnotations;
using DevOpsAPI.Messages;

namespace DevOpsAPI.Statics;

public class FileEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public MessageEntity? Message { get; set; }
}