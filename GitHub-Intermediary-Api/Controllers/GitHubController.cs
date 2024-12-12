using GitHub_Intermediary_Api.Framework;
using GitHub_Intermediary_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace GitHub_Intermediary_Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : ControllerBase {
        private static readonly List<string> AuthenticationTokens = [];

        [HttpGet("GenerateAccessToken")]
        public string GenerateAccessToken() {
            string accessToken = GenerateRandomAccessToken();
            while (AuthenticationTokens.Contains(accessToken)) accessToken = GenerateRandomAccessToken();
            AuthenticationTokens.Add(accessToken);
            return accessToken;
        }

        private static string GenerateRandomAccessToken() {
            Random random = new(); char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^*?".ToCharArray();
            char[] accessToken = new char[32];
            for (int i = 0; i < 32; i++) accessToken[i] = chars[random.Next(chars.Length)];
            return new string(accessToken);
        }

        [HttpGet("RetrieveUsers")]
        public string RetrieveUsersJson([FromQuery] List<string> usernames, [FromQuery] string accessToken) {
            if (AuthenticationTokens.Contains(accessToken)) {
                AuthenticationTokens.Remove(accessToken);
                ApiUserResponse apiUserResponse = RetrieveUsers(usernames);
                return JsonConvert.SerializeObject(apiUserResponse);
            } else return "Invalid Access Token";
        }

        [HttpGet("RetrieveUsersXml")]
        public string RetrieveUsersXml([FromQuery] List<string> usernames, [FromQuery] string accessToken) {
            if (AuthenticationTokens.Contains(accessToken)) {
                AuthenticationTokens.Remove(accessToken);
                ApiUserResponse apiUserResponse = RetrieveUsers(usernames);
                XmlSerializer xmlSerializer = new(apiUserResponse.GetType());
                using StringWriter stringWriter = new();
                xmlSerializer.Serialize(stringWriter, apiUserResponse);
                return stringWriter.ToString();
            } else return "Invalid Access Token";
        }

        private static ApiUserResponse RetrieveUsers(List<string> usernames) {
            usernames = ValidateUsernames(usernames, out Dictionary<string, string> errors);
            List<User> users = [];
            foreach (string username in usernames) {
                User? user = new ApiConnector().RetrieveUsersAsync(username).Result;
                if (user != null) users.Add(user);
                else {
                    if (!errors.TryAdd(username, "User not found.")) errors[username] = "User not found.";
                }
            }
            ApiUserResponse apiUserResponse = new() {
                Users = [.. users.OrderBy(u => u.Name)],
                Errors = errors.Select(e => new Error { Username = e.Key, Message = e.Value }).ToList()
            };
            return apiUserResponse;
        }

        private static List<string> ValidateUsernames(List<string> usernames, out Dictionary<string, string> errors) {
            List<string> validUsernames = []; errors = [];
            Regex regex = new(@"^[a-zA-Z0-9-]+$");
            foreach (string username in usernames) {
                if (regex.IsMatch(username)) {
                    if (!validUsernames.Contains(username)) validUsernames.Add(username);
                    else errors.Add(username, "Duplicate username found.");
                } else errors.Add(username, "Username is not in valid alphanumeric and hypen format.");
            }
            return validUsernames;
        }
    }
}
