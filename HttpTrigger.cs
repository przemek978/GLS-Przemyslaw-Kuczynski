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
        public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                
                

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task<bool> SendLabelsToPrinterAsync(string printerUrl, List<string> labels)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Ustawienia nag³ówków dla zapytania POST
                    var content = new StringContent(JsonConvert.SerializeObject(labels), Encoding.UTF8, "application/json");

                    // Wyœlij etykiety do drukarki za pomoc¹ zapytania POST
                    var response = await client.PostAsync(printerUrl, content);

                    // SprawdŸ, czy zapytanie zakoñczy³o siê sukcesem
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Logowanie ewentualnych b³êdów
                _logger.LogError($"Error sending labels to printer: {ex.Message}");
                return false;
            }
        }
    }
}
