using System;
using System.Collections.Generic;
using Common;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusQueueSettings serviceBusQueueSettings = new ServiceBusQueueSettings("YourAzureServiceBusEndPoint", "QueueName");
                      
            while (true)
            {
                new ServiceBusQueueSender<Order>(serviceBusQueueSettings).SendAsync(OrderBuilder.RandomOrder()).Wait();
            }
        }
    }
}
