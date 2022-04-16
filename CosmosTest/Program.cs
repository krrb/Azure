using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosTest
{
    public class Program
    {
        private static readonly string _endpointUrl = "https://rns-cosmos-db.documents.azure.com:443/";
        private static readonly string _key = "<Add Cosmos Account here>";
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        private string databaseId = "rnsfirstcosmosdb";
        private string containerId = "rnscontainer";

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...!\n");
                var p = new Program();
                await p.CosmosAsync();
            }
            catch(CosmosException de)
            {
                var baseException = de.GetBaseException();
                Console.WriteLine($"{de.StatusCode} error occured: {de}");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            finally
            {
                Console.WriteLine("End of program, press any key to exit");
                Console.ReadLine();
            }
        }

        private async Task CosmosAsync()
        {
            this.cosmosClient = new CosmosClient(_endpointUrl, _key);
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine($"Created database : {databaseId}");
        }

        private async Task CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine($"Created container {this.containerId}");
        }
    }
}