using GitHub_Intermediary_Api.Framework;
using GitHub_Intermediary_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GitHub_Intermediary_Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : ControllerBase {
        private static Dictionary<string, DateTime> AuthenticationTokens = [];

        [HttpGet("GenerateAccessToken")]
        public string GenerateAccessToken() {
            Generator generator = new();
            string accessToken = generator.GenerateRandomAccessToken();
            while (AuthenticationTokens.ContainsKey(accessToken)) accessToken = generator.GenerateRandomAccessToken();
            AuthenticationTokens.Add(accessToken, DateTime.UtcNow);
            return accessToken;
        }
        
        [HttpGet("RetrieveUsers")]
        public string RetrieveUsersJson([FromQuery] List<string> usernames, [FromQuery] string accessToken) {
            if (new Validator().ValidateAuthenticationToken(ref AuthenticationTokens, accessToken)) {
                ApiUserResponse apiUserResponse = RetrieveUsers(usernames);
                return JsonConvert.SerializeObject(apiUserResponse);
            }
            return "Invalid Access Token";
        }

        [HttpGet("RetrieveUsersXml")]
        public string RetrieveUsersXml([FromQuery] List<string> usernames, [FromQuery] string accessToken) {
            if (new Validator().ValidateAuthenticationToken(ref AuthenticationTokens, accessToken)) {
                ApiUserResponse apiUserResponse = RetrieveUsers(usernames);
                string xmlString = new Converter().ConvertToXml(apiUserResponse, "ApiUserResponse");
                return xmlString;
            }
            return "Invalid Access Token";
        }

        private static ApiUserResponse RetrieveUsers(List<string> usernames) {
            usernames = new Validator().ValidateUsernames(usernames, out Dictionary<string, string> errors);
            List<User> users = [];
            foreach (string username in usernames) {
                User? user = new ApiConnector().RetrieveUsersAsync(username).Result;
                if (user != null) users.Add(user);
                else errors.Add(username, "User not found.");
            }
            ApiUserResponse apiUserResponse = new() {
                Users = [.. users.OrderBy(u => u.Name)],
                Errors = errors.Select(e => new Error { Username = e.Key, Message = e.Value }).ToList()
            };
            return apiUserResponse;
        }
    }
}
