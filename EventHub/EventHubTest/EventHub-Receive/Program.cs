using Azure.Messaging.EventHubs.Consumer;
using System.Text;

namespace EventHub_Receive
{
    internal class Program
    {
        private static readonly string connection_string = "";
        private static readonly string consumer_group = "$Default";

        static async Task Main(string[] args)
        {
            EventHubConsumerClient _consumer_client = new EventHubConsumerClient(consumer_group, connection_string);

            string partitionId = (await _consumer_client.GetPartitionIdsAsync()).First();

            await foreach (PartitionEvent _event in _consumer_client.ReadEventsFromPartitionAsync(partitionId, EventPosition.Earliest))
            {
                Console.WriteLine($"Partition ID {_event.Partition.PartitionId}");
                Console.WriteLine($"Data Offset {_event.Data.Offset}");
                Console.WriteLine($"Sequence Number {_event.Data.SequenceNumber}");
                Console.WriteLine($"Partition Key {_event.Data.PartitionKey}");
                Console.WriteLine(Encoding.UTF8.GetString(_event.Data.EventBody));
            }

            Console.ReadKey();
        }
    }
}