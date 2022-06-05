using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace EventHub_Send
{
    internal class Program
    {
        private static readonly string connection_string = "";

        public static void Main(string[] args)
        {
            EventHubProducerClient _producer_client = new EventHubProducerClient(connection_string);
            EventDataBatch _batch = _producer_client.CreateBatchAsync().GetAwaiter().GetResult();

            for (int i = 0; i < 20; i++)
            {
                var _orders = new List<Order>()
                {
                    new Order() { OrderID = "01", Quantity = 10, UnitPrice = 9.99m, DiscountCategory = "Tire 01"},
                    new Order() { OrderID = "02", Quantity = 15, UnitPrice = 10.99m, DiscountCategory = "Tire 02"},
                    new Order() { OrderID = "03", Quantity = 20, UnitPrice = 11.99m, DiscountCategory = "Tire 03"},
                    new Order() { OrderID = "04", Quantity = 15, UnitPrice = 12.99m, DiscountCategory = "Tire 01"},
                    new Order() { OrderID = "05", Quantity = 30, UnitPrice = 13.99m, DiscountCategory = "Tire 02"}
                };



                foreach (var order in _orders)
                {
                    _batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(order.ToString())));
                }

                _producer_client.SendAsync(_batch).GetAwaiter().GetResult();
            }

            Console.WriteLine("Batch of events sent");
            Console.ReadKey();
        }
    }
}