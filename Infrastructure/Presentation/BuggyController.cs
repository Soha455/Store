using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]  // Get: /api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            return NotFound();   // 404
        }

        
        [HttpGet("servererror")]  // Get: /api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception(); // 500
            return Ok(); // 200
        }

        
        [HttpGet("badrequest")]  // Get: /api/Buggy/badrequest
        public IActionResult GetBadRequest()
        {
            return BadRequest();   // 400
        }

        
        [HttpGet("badrequest/{id}")]  // Get: /api/Buggy/badrequest/ahmed       ,Validation Error
        public IActionResult GetBadRequest(int id)
        {
            return BadRequest();   // 400
        }

        
        [HttpGet("unauthorized")]  // Get: /api/Buggy/unauthorized
        public IActionResult GetUnauthorizedRequest()
        {
            return Unauthorized();   // 401
        }
    }
}
