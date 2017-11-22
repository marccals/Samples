using System.Threading.Tasks;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Common;


namespace Producer
{
    ///Generic class to send an item to a ServiceBus queue
    public class ServiceBusQueueSender<T> where T: class
    {
        private readonly QueueClient _queueClient;

        public ServiceBusQueueSender(ServiceBusQueueSettings serviceBusQueueSettings)
        {
            _queueClient = new QueueClient(serviceBusQueueSettings.ConnectionString, serviceBusQueueSettings.QueueName);
        }

        /// Send message to the queue
        public async Task SendAsync(T item)
        {
            var itemAsJson = JsonConvert.SerializeObject(item);
            var message = new Message(Encoding.UTF8.GetBytes(itemAsJson));

            await _queueClient.SendAsync(message);
        }
    }
}