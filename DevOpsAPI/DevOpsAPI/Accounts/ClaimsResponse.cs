using System.Security.Claims;

namespace DevOpsAPI.Accounts;

public class ClaimsResponse
{
    public ClaimsIdentity Credentials { get; set; }
    public Guid Id { get; set; }
    public string JwtToken { get; set; }

    public ClaimsResponse(ClaimsIdentity credentials, Guid id, string jwtToken)
    {
        Credentials = credentials;
        Id = id;
        JwtToken = jwtToken;
    }
}