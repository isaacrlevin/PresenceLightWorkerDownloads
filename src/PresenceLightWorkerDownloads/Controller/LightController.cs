using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PresenceLightWorkerDownloads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : ControllerBase
    {
        public LightController(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetPresence")]
        public IActionResult GetPresence()
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var http = new HttpClient(handler);
            var res = http.GetAsync($"{Configuration["PiUrl"]}/api/Light/GetPresence");


            var result = res.Result.Content.ReadAsStringAsync().Result;

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet]

        public async Task<string> UpdateLight(string command)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var http = new HttpClient(handler);
            var res = http.GetAsync($"{Configuration["PiUrl"]}/api/Light?command={command}");

            return $"Lights set to {command}";
        }
    }
}
