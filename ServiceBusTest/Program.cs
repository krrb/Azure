using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace AzureServiceBusTest
{
    class Program
    {
        static string connectionString = "Endpoint=sb://rnsaz204servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ta0SqXoeeH6MB8yM8KXL0xKAG76PWZDhlc08izFS2EM=";
        static string queueName = "rns-queue";
        static ServiceBusClient serviceBusClient;
        static ServiceBusSender serviceBusSender;
        static ServiceBusProcessor serviceBusProcessor;
        private const int numberOfMessages = 3;

        static async Task Main()
        {
            // // Create the clients that we'll use for sending and processing messages.
            // serviceBusClient = new ServiceBusClient(connectionString);
            // serviceBusSender = serviceBusClient.CreateSender(queueName);

            // // create a batch 
            // using ServiceBusMessageBatch messageBatch = await serviceBusSender.CreateMessageBatchAsync();

            // for (int i = 1; i <= 3; i++)
            // {
            //     // try adding a message to the batch
            //     if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
            //     {
            //         throw new Exception($"Exception {i} has occured");
            //     }
            // }

            // try
            // {
            //     // Use the producer client to send the batch of messages to the Service Bus queue
            //     await serviceBusSender.SendMessagesAsync(messageBatch);
            //     Console.WriteLine($"A batch of {numberOfMessages} messages has been published to the queue.");
            // }
            // finally
            // {
            //     await serviceBusSender.DisposeAsync();
            //     await serviceBusClient.DisposeAsync();
            // }

            // Console.WriteLine("Press any key to end the application");
            // Console.ReadKey();

            // _________________________________________________________________________________________________________

            // Create the client object that will be used to create sender and receiver objects
            serviceBusClient = new ServiceBusClient(connectionString);

            // create a processor that we can use to process the messages
            serviceBusProcessor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                serviceBusProcessor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

                // start processing
                await serviceBusProcessor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await serviceBusProcessor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await serviceBusProcessor.DisposeAsync();
                await serviceBusClient.DisposeAsync();
            }
        }

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Recieved: {body}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}