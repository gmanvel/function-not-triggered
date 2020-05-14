using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionServiceBusTriggerNotExecuted
{
    public static class NotTriggeredFunction
    {
        [FunctionName("NotTriggeredFunction")]
        public static void Run(
            [ServiceBusTrigger(QueueNames.TriggerQueue, Connection = Constants.ServiceBusConnectionString)]Message message, 
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {message.MessageId}");
        }
    }
}
