using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevOpsAPI.DAL;
using DevOpsAPI.Infra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DevOpsAPI.Accounts;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly DataContext db;

    public AccountsController(DataContext db)
    {
        this.db = db;
    }

    [HttpPost("Auth")]
    public async Task<ActionResult<ClaimsResponse>> Auth(RegReq req)
    {
        var existed = db.Accounts.FirstOrDefault(e => e.Login == req.Login && e.Password == req.Password);
        if (existed == null)
            return BadRequest("Incorrect login/password");

        return Ok(GetClaims(existed));
    }

    [HttpPost("Reg")]
    public async Task<ActionResult<ClaimsResponse>> Reg(RegReq req)
    {
        var existed = db.Accounts.FirstOrDefault(e => e.Login == req.Login);
        if (existed != null)
            return BadRequest("Already existed");

        var newAcc = new AccountEntity
        {
            Login = req.Login,
            Password = req.Password,
        };
        await db.Accounts.AddAsync(newAcc);
        await db.SaveChangesAsync();
        return await Auth(req);
    }

    public class RegReq
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    
    private ClaimsResponse GetClaims(AccountEntity account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsResponse(claimsIdentity, account.Id, CreateToken(claims));
    }

    public string CreateToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            Config.JwtSecurityKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}