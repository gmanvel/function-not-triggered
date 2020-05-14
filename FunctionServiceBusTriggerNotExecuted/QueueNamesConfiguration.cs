using System.Collections.Generic;

namespace FunctionServiceBusTriggerNotExecuted
{
    public class QueueNamesConfiguration
    {
        public Dictionary<string, string> Values { get; set; }

        public virtual string GetQueueName(string stepName)
        {
            var key = stepName.Replace("%", string.Empty);

            if (Values != null)
            {
                Values.TryGetValue(key, out var value);

                if (value != null)
                {
                    return value;
                }
            }

            return key;
        }
    }
}