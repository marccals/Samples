using System;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        private const String ServiceBusConnectionString = "YourEndpoint";
        private const String QueueName = "SimpleQueue";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHanler(ExceptionReceivedEventArgs exceptionReceivedEventArgs) 
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            return Task.CompletedTask;
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHanler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            };

            queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            for (var i = 0; i < numberOfMessagesToSend; i++)
            {
                try
                {
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    Console.WriteLine($"Sending message: {messageBody}");

                    await queueClient.SendAsync(message);
                }
                catch(Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                }
            }
        }

        static async Task MainAsync(string[] args)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            RegisterOnMessageHandlerAndReceiveMessages();


            await SendMessagesAsync(10);

            Console.WriteLine("Press any key");
            Console.ReadKey();
            await queueClient.CloseAsync();
        }

    }
}
