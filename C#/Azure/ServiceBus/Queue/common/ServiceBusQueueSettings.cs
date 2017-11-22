using System;

namespace Common
{
    //Class containts Azure Service bus queue settings
    public class ServiceBusQueueSettings
    {
        public string ConnectionString { get; }
        public string QueueName { get; }

        public ServiceBusQueueSettings(string connectionString, string queueName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString is null or empty");
            }

            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException("queueName is null or empty");
            }

            ConnectionString = connectionString;
            QueueName = queueName;
        }
    }
}
