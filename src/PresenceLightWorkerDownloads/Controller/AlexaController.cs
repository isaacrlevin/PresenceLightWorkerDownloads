using System;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PresenceLightWorkerDownloads.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AlexaController : ControllerBase
    {
        public AlexaController(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ProcessAlexaRequest([FromBody] SkillRequest request)
        {
            var requestType = request.GetRequestType();
            SkillResponse response = null;

            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var http = new HttpClient(handler);


            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Welcome to Presence Light!");
                response.Response.ShouldEndSession = false;
            }
            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = request.Request as IntentRequest;

                if (intentRequest.Intent.Name == "Teams")
                {
                    var res = http.GetAsync($"{Configuration["PiUrl"]}/api/Light?command={intentRequest.Intent.Name}");
                    response = ResponseBuilder.Tell("Presence Light set to Teams!");
                }
                else if (intentRequest.Intent.Name == "Custom")
                {
                    var res = http.GetAsync($"{Configuration["PiUrl"]}/api/Light?command={intentRequest.Intent.Name}");
                    response = ResponseBuilder.Tell("Presence Light set to custom!");
                }
            }

            return new OkObjectResult(response);
        }

    }
}
