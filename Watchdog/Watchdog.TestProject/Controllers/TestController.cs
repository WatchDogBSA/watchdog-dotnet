using Microsoft.AspNetCore.Mvc;
using System;

namespace Watchdog.TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("{number}")]
        public ActionResult Test(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Number can't be less than 0", nameof(number));
            }
            return Ok();
        }
    }
}
