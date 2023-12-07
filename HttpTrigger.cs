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

namespace GLS_Przemyslaw_Kuczynski
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("HttpTrigger")]
        public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                //var response = req.CreateResponse(HttpStatusCode.OK);

                //var packages = _dbContext.Packages.ToList();

                //var packagesNumbers = new List<string>();
                //foreach (var package in packages)
                //{
                //    packagesNumbers.Add($"Package ID: {package.Id}, Tracking Number: {package.PackageNumber}");
                //}

                //// Wyœlij etykiety do drukarki
                //var printerUrl = "https://moja-drukarka.pl/print";
                //var result = await SendLabelsToPrinterAsync(printerUrl, packagesNumbers);

                //if (result)
                //{
                //    return new OkObjectResult("Labels sent to printer successfully.");
                //}
                //else
                //{
                //    return new BadRequestObjectResult("Failed to send labels to printer.");
                //}

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
