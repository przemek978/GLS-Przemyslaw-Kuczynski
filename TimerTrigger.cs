using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Triggers.Data;
using Triggers.Services;

namespace GLS_Przemyslaw_Kuczynski
{
    public class TimerTrigger
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;

        public TimerTrigger(ILoggerFactory loggerFactory, IDataService dataService, DBContext dBContext)
        {
            _logger = loggerFactory.CreateLogger<TimerTrigger>();
            _dataService = dataService;
        }

        [Function("TimerTrigger")]
        public async Task Run([TimerTrigger("0 */10 * * * *")] TimerInfo myTimer)
        {
            try
            {
                var sessionId = await _dataService.Login();
                _logger.LogInformation($"Session id to {sessionId}");
                var packagesIds = await _dataService.GetPackageIds(sessionId,0);
                var convertedIds = ConvertHexStringsToIntegers(packagesIds);
                var labels = await _dataService.GetLabels(sessionId,convertedIds,"");
                _dataService.InsertLabels(convertedIds, labels);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogInformation("B³¹d w trakcie wysy³ania ¿¹dania: " + ex.Message);
            }

        }

        static List<int> ConvertHexStringsToIntegers(List<string> hexStrings)
        {
            List<int> intList = new List<int>();

            foreach (string hexString in hexStrings)
            {
                if (int.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out int intValue))
                {
                    intList.Add(intValue);
                }
                else
                {
                    Console.WriteLine($"Nieudana konwersja dla identyfikatora: {hexString}");
                }
            }

            return intList;
        }
    }
}
