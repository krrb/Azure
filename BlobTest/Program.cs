using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobTest
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Azure Blob Storage exercise\n");
            
            ProcessAsync().GetAwaiter().GetResult();

            Console.WriteLine("Press enter to exit sample application");
            Console.ReadLine();
        }

        private static async Task ProcessAsync()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=rnsblobtest;AccountKey=ksfGDLW81jRmA5fwaq+5gGuGeZOCGGcQIq57QIOJ8GHG+HRyu3ucehH28tTVorkAE0UNjj60Y55SDQMyeDYb+w==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            string containerName = "wtblob" + Guid.NewGuid().ToString();

            // Create a container
            BlobContainerClient blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            Console.WriteLine($"Container named '{containerName}' created.");

            // Create a text file
            string localPath = "./data";
            string fileName =  $"wtFile-{Guid.NewGuid().ToString()}.txt";
            string localFilePath = Path.Combine(localPath, fileName);
            await File.WriteAllTextAsync(localFilePath, "This is a test text. My Name is Rajeev Banda");

            // Create a blob
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            Console.WriteLine($"Uploading to blob storage:\t {blobClient.Uri}");

            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            Console.WriteLine("File uploaded to blob stroage");

            // Download blob from blob storage
            string downloadedFilePath = localFilePath.Replace(".txt", "DOWNLOAD.txt");
            Console.WriteLine($"Downloading file from blob");
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            using(FileStream downloadFileStream = File.OpenWrite(downloadedFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }

            Console.WriteLine("File downloaded");

            // Delete Blob
            await blobClient.DeleteAsync();
            Console.WriteLine($"Blob '{blobClient.Name}' Deleted");

            // Delete blob container
            await blobContainerClient.DeleteAsync();
            Console.WriteLine($"Blob container '{blobContainerClient.Name}' deleted");


            Console.WriteLine("Press 'Enter' to continue.");
            Console.ReadLine();
        }
    }
}