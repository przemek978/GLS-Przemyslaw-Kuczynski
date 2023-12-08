using GlsAPI.Interfaces;
using GlsAPI.Models.Responses;
using GlsAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GlsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(Endpoints.Login)]
        public ActionResult<AuthResponse> Login(string username, string password)
        {
            try
            {
                AuthResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _userService.Login(username, password);
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost(Endpoints.Logout)]
        public ActionResult<AuthResponse> Logout(Guid session)
        {
            try
            {
                AuthResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _userService.Logout(session);
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
