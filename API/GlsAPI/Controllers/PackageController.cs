using GlsAPI.Interfaces;
using GlsAPI.Models;
using GlsAPI.Models.Requests;
using GlsAPI.Models.Responses;
using GlsAPI.Services;
using GlsAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet(Endpoints.GetConsignIDs)]
        public ActionResult<PackagesResponse> GetConsignIDs(Guid session, int id_start)
        {
            try
            {
                PackagesResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _packageService.GetPackagesIDs(session, id_start);
                }
                if (response.Error == null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet(Endpoints.GetConsign)]
        public ActionResult<PackageResponse> GetConsign(Guid session, int id)
        {
            try
            {
                PackageResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _packageService.GetPackage(session, id);
                }
                if (response.Error == null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost(Endpoints.GetConsignLabels)]
        public ActionResult<LabelResponse> GetConsignLabels([FromBody]LabelsRequest req)
        {
            try
            {
                LabelResponse response = new();
                if (ModelState.IsValid)
                {
                    response = _packageService.GetConsignLabels(req.sessionId, req.packageIds, req.mode);
                }
                if (response.Error == null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response); 
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
