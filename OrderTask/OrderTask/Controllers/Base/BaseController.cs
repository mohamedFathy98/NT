using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderTask.Controllers.Base
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
