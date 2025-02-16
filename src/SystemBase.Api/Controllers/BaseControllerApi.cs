

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBase.Api.Filters;

namespace SystemBase.Api.Controllers;

[Route("api/[controller]")]
[ApiResultFilter]
[ApiController]
[Authorize]
public class BaseControllerApi : ControllerBase
{

}
