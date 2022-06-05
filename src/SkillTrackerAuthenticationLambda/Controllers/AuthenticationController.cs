using Microsoft.AspNetCore.Mvc;
using SkillTrackerAuthenticationLambda.AuthManager;
using SkillTrackerAuthenticationLambda.Model;

namespace SkillTrackerAuthenticationLambda.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthenticationController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody]UserCred userCred)
        {
            if (userCred == null)
                return BadRequest();

            var token = _jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
