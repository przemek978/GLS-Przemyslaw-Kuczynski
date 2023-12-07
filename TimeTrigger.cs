using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GLS_Przemyslaw_Kuczynski
{
    public class TimerTrigger
    {
        private readonly ILogger _logger;

        public TimerTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("TimerTrigger")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            //_glsService.SynchronizePackages();
        }


    }
}
