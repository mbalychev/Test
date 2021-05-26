using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            logger = _logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            logger.LogInformation("requst file");
            HttpResponseMessage response = null;
            try
            {
            response = await client.GetAsync("http://localhost:29461/weatherforecast");
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("file is get");
                    var res = await response.Content.ReadAsByteArrayAsync();
                    using (FileStream fstream = new FileStream($"C:\\Users\\Михаил\\Downloads\\note.csv", FileMode.OpenOrCreate))
                    {
                        fstream.Write(res, 0, res.Length);
                    }
                }
                else
                {
                    logger.LogError("file is not getting");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"connecting error: {e.Message}");
            }
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
