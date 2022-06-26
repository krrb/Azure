using AzureADTestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureADTestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.PrincipalName = this.HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"].ToString();
            ViewBag.PrincipalID = this.HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
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