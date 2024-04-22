using System.ComponentModel.DataAnnotations;

namespace DevOpsAPI.Accounts;

public class AccountEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}