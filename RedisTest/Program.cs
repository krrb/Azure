using StackExchange.Redis;

namespace BlobTest
{
    class Program
    {
        private static readonly string connectionString = "rns-redis-001.redis.cache.windows.net:6380,password=YcxyCBq88Sok59ks3c7hZqiMZiMkDJyKTAzCaPa1RBc=,ssl=True,abortConnect=False";
        
        static async Task Main()
        { 
            using (var cache = ConnectionMultiplexer.Connect(connectionString))
            {
                IDatabase db = cache.GetDatabase();

                var result = await db.ExecuteAsync("ping");
                Console.WriteLine($"PING = {result.Type} : {result}");

                bool setValue = await db.StringSetAsync("test:key", "100");
                Console.WriteLine($"SET : {setValue}");

                string getValue = await db.StringGetAsync("test:key");
                Console.WriteLine($"GET : {getValue}");
            }
        }
    }
}