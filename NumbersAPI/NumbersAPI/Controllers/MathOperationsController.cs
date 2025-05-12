using Microsoft.AspNetCore.Mvc;
using NumbersAPI.DataServiceLayer;

namespace NumbersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathOperationsController : ControllerBase
    {
        private readonly AddNumbersDSL addNumbersDSL;
        private readonly SubtractNumberDSL subtractNumberDSL;

        public MathOperationsController(AddNumbersDSL addNumbersDSL, SubtractNumberDSL subtractNumberDSL) //responsible for handling HTTP requests
        {
            this.addNumbersDSL = addNumbersDSL;
            this.subtractNumberDSL = subtractNumberDSL;
        }

        [HttpGet("AddNumbers")] //endpoint For Adding
        public IActionResult Sum(int a, int b)
        {
            var result = addNumbersDSL.Sum(a, b); // Calls the Sum method in AddNumbersDSL
            return Ok(result);
        }

        [HttpPost("subtractNumber")] //endpoint For subtract
        public IActionResult Subtract(int a, int b)
        {
            var result = subtractNumberDSL.Subtract(a, b); // Calls the Subtract method in SubtractNumberDSL
            return Ok(result);
        }

    }
}
