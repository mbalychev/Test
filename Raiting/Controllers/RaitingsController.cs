using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Raiting.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class RatingsController : ApiController
    {
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(ILogger<RatingsController> logger)
        {
            _logger = logger;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Get()
        {
            string path = @"c:\raiting.csv";
            byte[] b = new byte[1024];
            using (FileStream fstream = File.OpenRead(path))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                await fstream.ReadAsync(array, 0, array.Length);

                return new FileContentResult(array,
                MimeTypeMap.GetMimeType("csv"))
                {
                    FileDownloadName = "raiting.csv"
                };
            }
        }
    }
}
