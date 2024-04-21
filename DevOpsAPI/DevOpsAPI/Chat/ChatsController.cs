using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsAPI.Chat;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatsController : ControllerBase
{
    
}