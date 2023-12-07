using GlsAPI.Interfaces;
using GlsAPI.Models.Responses;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("adeLogin")]
        public AuthResponse Login(string username, string password)
        {
            try
            {
                AuthResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _userService.Login(username, password);
                }
                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("adeLogout")]
        public AuthResponse Logout(Guid session)
        {
            try
            {
                AuthResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _userService.Logout(session);
                }
                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
