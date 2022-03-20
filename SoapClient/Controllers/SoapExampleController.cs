using Microsoft.AspNetCore.Mvc;
using SoapClient.SoapClientHandler;

namespace SoapClient.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class SoapExampleController : ControllerBase
    {
        private readonly ISoapClientService _soapClientService;

        public SoapExampleController(ISoapClientService soapClientService)
        {
            _soapClientService = soapClientService;
        }

        [Route("SoapClient/sendRequest")]
        [HttpGet]
        [ActionName("NetimNameSuggest")]
        public async Task<IActionResult> Get(string url, string action, string xmlString)
        {
            var data = await Task.Run(() => _soapClientService.CallWebService(url,action,xmlString));
            return Ok(data);
        }
    }
}
