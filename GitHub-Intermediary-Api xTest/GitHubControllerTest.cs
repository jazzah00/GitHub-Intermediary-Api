using GitHub_Intermediary_Api.Controllers;
using GitHub_Intermediary_Api.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace GitHub_Intermediary_Api_xTest {
    public class GitHubControllerTest {

        [Fact]
        public void RetrieveUsers_AccessToken_Invalid() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah00", "Octacat"];

            string result = gitHubController.RetrieveUsersXml(usernames, "abc123");
            Assert.NotEmpty(result);
            Assert.Equal("Invalid Access Token", result);
        }

        [Fact]
        public void RetrieveUsers_AccessToken_Timeout() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah00", "Octacat"];

            string result = gitHubController.GenerateAccessToken();
            Assert.NotEmpty(result);
            Thread.Sleep(70 * 1000);

            result = gitHubController.RetrieveUsersXml(usernames, result);
            Assert.NotEmpty(result);
            Assert.Equal("Invalid Access Token", result);
        }

        [Fact]
        public void RetrieveUsers_Valid_Xml() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah00", "Octacat"];

            string result = gitHubController.GenerateAccessToken();
            Assert.NotEmpty(result);
            result = gitHubController.RetrieveUsersXml(usernames, result);
            Assert.NotNull(result);

            if (!string.IsNullOrEmpty(result)) {
                using (StringReader reader = new(result)) {
                    XmlSerializer xmlSerializer = new(typeof(ApiUserResponse));
                    ApiUserResponse response = (ApiUserResponse)xmlSerializer.Deserialize(reader);

                    Assert.NotNull(response);
                    Assert.Equal(2, response.Users.Count);
                }
            }
        }

        [Fact]
        public void RetrieveUsers_Valid_Json() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah00", "Octacat"];

            string result = gitHubController.GenerateAccessToken();
            Assert.NotEmpty(result);
            result = gitHubController.RetrieveUsersJson(usernames, result);
            Assert.NotNull(result);

            if (!string.IsNullOrEmpty(result)) {
                ApiUserResponse response = JsonConvert.DeserializeObject<ApiUserResponse>(result);
                Assert.NotNull(response);
                Assert.Equal(2, response.Users.Count);
            }
        }

        [Fact]
        public void RetrieveUsers_Valid_With_Duplicate() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah00", "Jazzah00", "Octadog"];

            string result = gitHubController.GenerateAccessToken();
            Assert.NotEmpty(result);
            result = gitHubController.RetrieveUsersJson(usernames, result);
            Assert.NotNull(result);

            if (!string.IsNullOrEmpty(result)) {
                ApiUserResponse response = JsonConvert.DeserializeObject<ApiUserResponse>(result);
                Assert.NotNull(response);
                Assert.Equal(2, response.Users.Count);
                Assert.Single(response.Errors);
            }
        }

        [Fact]
        public void RetrieveUsers_Empty_With_Errors() {
            GitHubController gitHubController = new();
            List<string> usernames = ["Jazzah 00", "petepenguin"];

            string result = gitHubController.GenerateAccessToken();
            Assert.NotEmpty(result);
            result = gitHubController.RetrieveUsersJson(usernames, result);
            Assert.NotNull(result);

            if (!string.IsNullOrEmpty(result)) {
                ApiUserResponse response = JsonConvert.DeserializeObject<ApiUserResponse>(result);
                Assert.NotNull(response);
                Assert.Empty(response.Users);

                Assert.Equal(2, response.Errors.Count);
                Assert.Single(response.Errors.Where(e => e.Message.Equals("Username is not in valid alphanumeric and hypen format.")).ToList());
                Assert.Single(response.Errors.Where(e => e.Message.Equals("User not found.")).ToList());
            }
        }
    }
}