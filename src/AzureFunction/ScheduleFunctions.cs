using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunction
{
    public class ScheduleFunctions(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<ScheduleFunctions>();

        [Function("ScheduleFunctions")]
        public void Run([TimerTrigger("5,10,14 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
