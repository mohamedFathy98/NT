using Microsoft.AspNetCore.Mvc;
using NumbersAPI.DataServiceLayer;
using NumbersAPI.Models;

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

        [HttpPost("AddNumbers")] //endpoint For Adding
        public IActionResult AddNumbers([FromBody] NumberInput model)
        {
            int sum = model.a + model.b;
            return Ok(new { sum });
        }

        [HttpPost("subtractNumber")] //endpoint For subtract
        public IActionResult subtractNumbers([FromBody] NumberInput model)
        {
            int sub = model.a - model.b;
            return Ok(new { sub });
        }

    }
}
