using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Diagnostics;
using WebAppAzureAuthTets.Models;

namespace WebAppAzureAuthTets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;

        public HomeController(ILogger<HomeController> logger,
                              ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task<IActionResult> Index()
        {
            //string[] scopes = new string[] { "https://storage.azure.com/user_impersonation" };
            //string accesstoken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            //ViewBag.accesstoken = accesstoken.ToString();


            //Uri blobUri = new Uri("https://rnsauthstorage.blob.core.windows.net/data/Temp.txt");

            //TokenAcquisitionTokenCredential credential = new TokenAcquisitionTokenCredential(_tokenAcquisition);
            //BlobClient blobClient = new BlobClient(blobUri, credential);

            //MemoryStream ms = new MemoryStream();
            //blobClient.DownloadTo(ms);
            //ms.Position = 0;

            //StreamReader _reader = new StreamReader(ms);
            //string content = _reader.ReadToEnd();
            //ViewBag.content = content;
            //_reader.Close();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}