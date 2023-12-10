using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MojaDrukarka.Controllers
{
    [Route("/")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        [HttpPost("print")]
        public void print(IFormFile formFile)
        {
            var a = (IFormFile)formFile;
        }
    }
}
