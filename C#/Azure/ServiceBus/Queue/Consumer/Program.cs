using System;
using Common;
using Newtonsoft.Json;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusQueueSettings serviceBusQueueSettings = new ServiceBusQueueSettings("YourAzureServiceBusEndPoint", "QueueName");

            ServiceBusQueueReceiver<Order> serviceBusQueueReceiver = new ServiceBusQueueReceiver<Order>(serviceBusQueueSettings);

            serviceBusQueueReceiver.Receive(message => {
                string messageAsJson = JsonConvert.SerializeObject(message);

                Console.WriteLine(messageAsJson);

                return MessageProcessResponse.Complete;
            },
            ex => 
            {
                Console.WriteLine(ex.Message);
            },
            () => {
                Console.WriteLine("Waiting...");
            });

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
