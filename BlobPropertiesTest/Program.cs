using Azure;
using Azure.Storage.Blobs;

namespace BlobPropertiesTest
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("View blob properties");
            var blobConnectionString = "BLOB Connection string here";
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("rns-test-container");

            ReadContainerPropeties(blobContainerClient).GetAwaiter().GetResult();
            SetMetaData(blobContainerClient).GetAwaiter().GetResult();
            GetMetaData(blobContainerClient).GetAwaiter().GetResult();
        }


        private static async Task ReadContainerPropeties(BlobContainerClient blobContainerClient)
        {

            try
            {
                var containerPropeties = await blobContainerClient.GetPropertiesAsync();
                Console.WriteLine($"Properties for container : {blobContainerClient.Uri}");
                Console.WriteLine($"Public access level :  {containerPropeties.Value.PublicAccess}");
                Console.WriteLine($"Last modified time : {containerPropeties.Value.LastModified}");
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        private static async Task SetMetaData(BlobContainerClient blobContainerClient)
        {
            try
            {
                Console.WriteLine("Adding new meta data");
                IDictionary<string, string> metaData = new Dictionary<string, string>();
                metaData.Add("docType", "textDocument");
                metaData.Add("category", "test");

                await blobContainerClient.SetMetadataAsync(metaData);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

        private static async Task GetMetaData(BlobContainerClient blobContainerClient)
        {
            try
            {
                var properties = await blobContainerClient.GetPropertiesAsync();
                Console.WriteLine("Container Metadata");

                foreach (var metadataItem in properties.Value.Metadata)
                {
                    Console.WriteLine($"\tKey: {metadataItem.Key}");
                    Console.WriteLine($"\tValue: {metadataItem.Value}");
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}