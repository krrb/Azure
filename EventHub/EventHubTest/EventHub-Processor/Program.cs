using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System.Text;

namespace EventHub_Processor
{
    internal class Program
    {
        private static readonly string connection_string = "";
        private static readonly string consumer_group = "$Default";
        private static readonly string storage_account_connction = "";
        private static readonly string container_name = "eventhub";


        static async Task Main(string[] args)
        {
            BlobContainerClient _container_client = new BlobContainerClient(storage_account_connction, container_name);
            EventProcessorClient _processor = new EventProcessorClient(_container_client, consumer_group, connection_string);

            _processor.ProcessErrorAsync += OurErrorHandler;
            _processor.ProcessEventAsync += OurEventHandler;

            await _processor.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(30));

            Console.ReadKey();
        }

        static async Task OurEventHandler(ProcessEventArgs eventArgs)
        {
            Console.WriteLine($"Sequence number {eventArgs.Data.SequenceNumber}");
            Console.WriteLine(Encoding.UTF8.GetString(eventArgs.Data.EventBody));
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task OurErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}