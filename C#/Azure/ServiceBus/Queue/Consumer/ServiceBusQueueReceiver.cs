using Common;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class ServiceBusQueueReceiver<T> where T:class
    {
        private readonly QueueClient _queueClient;
        public ServiceBusQueueReceiver(ServiceBusQueueSettings serviceBusQueueSettings)
        {
            _queueClient = new QueueClient(serviceBusQueueSettings.ConnectionString, serviceBusQueueSettings.QueueName);
        }

        public void Receive(Func<T, MessageProcessResponse> onProcess, Action<Exception> onError, Action onWait)
        {
            var options = new MessageHandlerOptions(e => 
            {
                onError(e.Exception);
                return Task.CompletedTask;
            })
            {
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(
                async(message, token) =>
                {
                    try
                    {
                        var data = Encoding.UTF8.GetString(message.Body);
                        T item = JsonConvert.DeserializeObject<T>(data);

                        var result = onProcess(item);

                        switch(result)
                        {
                            case MessageProcessResponse.Complete:
                                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                                break;
                            case MessageProcessResponse.Abandon:
                                await _queueClient.AbandonAsync(message.SystemProperties.LockToken);
                                break;
                            case MessageProcessResponse.Dead:
                                await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken);
                                break;
                        }

                        //waiting for next message
                        onWait();
                    }
                    catch (Exception e)
                    {
                        await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken);
                        onError(e);
                    }
                }, options);
        }
    }
}