using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net.Http.Json;
using Triggers.Services;

namespace GLS_Przemyslaw_Kuczynski
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;

        public HttpTrigger(ILoggerFactory loggerFactory, IDataService dataService)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
            _dataService = dataService; 
        }

        [Function("HttpTrigger")]
        public async Task<ActionResult<string>> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
               var message = _dataService.Print();
               _logger.LogInformation(message);
               return new OkObjectResult(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
