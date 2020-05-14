using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace FunctionServiceBusTriggerNotExecuted
{
    public class CustomNameResolver : INameResolver
    {
        private readonly QueueNamesConfiguration _queueNamesConfigurationOptions;

        public CustomNameResolver(IConfigurationRoot appSettingsConfig)
        {
            _queueNamesConfigurationOptions = new QueueNamesConfiguration();

            appSettingsConfig.GetSection("QueueNamesConfiguration").Bind(_queueNamesConfigurationOptions);
        }

        public string Resolve(string name)
        {
            if (name.Contains("ConnectionString"))
                return name;

            return _queueNamesConfigurationOptions.GetQueueName(name);
        }
    }
}