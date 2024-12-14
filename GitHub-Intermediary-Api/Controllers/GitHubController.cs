using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GitHub_Intermediary_Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GitHubController : ControllerBase {
        private readonly IGitHubService _GitHubService;
        private readonly IConverter _Converter;

        public GitHubController(IGitHubService gitHubService, IConverter converter) {
            _GitHubService = gitHubService;
            _Converter = converter;
        }

        [HttpGet("RetrieveUsersJson")]
        public async Task<IActionResult> RetrieveUsersJson([FromQuery] List<string> usernames) {
            ApiUserResponse apiUserResponse = await _GitHubService.RetrieveUsersAsync(usernames);
            return Ok(JsonConvert.SerializeObject(apiUserResponse));
        }

        [HttpGet("RetrieveUsersXml")]
        public async Task<IActionResult> RetrieveUsersXml([FromQuery] List<string> usernames) {
            ApiUserResponse apiUserResponse = await _GitHubService.RetrieveUsersAsync(usernames);
            string xmlString = _Converter.ConvertToXml(apiUserResponse, "ApiUserResponse");
            return Ok(xmlString);
        }

        [HttpGet("TestAuth")]
        public IActionResult TestAuth() => Ok("Token is Valid and request authorized.");
    }
}
