using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace MojaDrukarka.Controllers
{
    [Route("/")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        [HttpPost("print")]
        public IActionResult Print(IFormFile file)
        {
            try
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                return File(fileBytes, "application/octet-stream", file.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}
