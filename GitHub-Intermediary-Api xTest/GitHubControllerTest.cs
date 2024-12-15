using GitHub_Intermediary_Api.Controllers;
using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace GitHub_Intermediary_Api_xTest {
    public class GitHubControllerTest {
        private readonly Mock<IGitHubService> _MockGitHubService;
        private readonly Mock<IConverter> _MockConverter;
        private readonly GitHubController _GitHubController;

        public GitHubControllerTest() {
            _MockGitHubService = new Mock<IGitHubService>();
            _MockConverter = new Mock<IConverter>();
            _GitHubController = new GitHubController(_MockGitHubService.Object, _MockConverter.Object);
        }

        [Fact]
        public async Task RetrieveUsersJson_ReturnsOkResult_WithSerializedJson() {
            List<string> usernames = ["arrow", "spartan"];
            ApiUserResponse apiUserResponse = new() {
                Users = [
                    new User {
                        Name = "John Diggle",
                        Login = "spartan",
                        Company = "US Army",
                        Followers = 170,
                        Public_Repos = 8 },
                    new User {
                        Name = "Oliver Queen",
                        Login = "arrow",
                        Company = "Queen Industries",
                        Followers = 170,
                        Public_Repos = 8
                    },
                    new User {
                        Name = "Felicity Smoak",
                        Login = "Overwatch",
                        Company = "",
                        Followers = 160,
                        Public_Repos = 8
                    }
                ],
                Errors = []
            };
            string jsonString = JsonConvert.SerializeObject(apiUserResponse);
            _MockGitHubService.Setup(gh => gh.RetrieveUsersAsync(usernames)).ReturnsAsync(apiUserResponse);

            IActionResult? result = await _GitHubController.RetrieveUsersJson(usernames);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(jsonString, okObjectResult.Value);
        }

        [Fact]
        public async Task RetrieveUsersJson_ReturnsOkResult_WithSerializedXml() {
            List<string> usernames = ["arrow", "spartan"];
            ApiUserResponse apiUserResponse = new() {
                Users = [
                    new User {
                        Name = "Felicity Smoak",
                        Login = "Overwatch",
                        Company = "",
                        Followers = 160,
                        Public_Repos = 8
                    },
                    new User {
                        Name = "John Diggle",
                        Login = "Spartan",
                        Company = "US Army",
                        Followers = 170,
                        Public_Repos = 8 },
                    new User {
                        Name = "Oliver Queen",
                        Login = "Arrow",
                        Company = "Queen Industries",
                        Followers = 170,
                        Public_Repos = 8
                    }
                ],
                Errors = []
            };
            string xmlResponse = "<ApiUserResponse><Users><User><Name>Felicity Smoak</Name><Login>Overwatch</Login><Company></Company><Followers>160</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>20</Average_Followers_Per_Repository></User><User><Name>John Diggle</Name><Login>Spartan</Login><Company>US Army</Company><Followers>170</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>23</Average_Followers_Per_Repository></User><User><Name>Oliver Queen</Name><Login>Arrow</Login><Company>Queen Industries</Company><Followers>170</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>23</Average_Followers_Per_Repository></User></Users><Errors></Errors></ApiUserResponse>";
            _MockGitHubService.Setup(gh => gh.RetrieveUsersAsync(usernames)).ReturnsAsync(apiUserResponse);
            _MockConverter.Setup(c => c.ConvertToXml(apiUserResponse, "ApiUserResponse")).Returns(xmlResponse);

            IActionResult? result = await _GitHubController.RetrieveUsersXml(usernames);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(xmlResponse, okObjectResult.Value);
        }

        [Fact]
        public void TestAuth_ReturnsOkResult_WithMessage() {
            IActionResult? result = _GitHubController.TestAuth();

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Token is Valid and request authorized.", okObjectResult.Value);
        }
    }
}