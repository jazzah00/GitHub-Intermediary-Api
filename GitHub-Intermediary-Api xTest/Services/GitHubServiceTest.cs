using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Models;
using GitHub_Intermediary_Api.Services;
using Moq;

namespace GitHub_Intermediary_Api_xTest.Services {
    public class GitHubServiceTest {
        private readonly Mock<IApiConnector> _ApiConnectorMock;
        private readonly Mock<IValidator> _ValidatorMock;
        private readonly GitHubService _GitHubService;

        public GitHubServiceTest() {
            _ApiConnectorMock = new Mock<IApiConnector>();
            _ValidatorMock = new Mock<IValidator>();
            _GitHubService = new GitHubService(_ApiConnectorMock.Object, _ValidatorMock.Object);
        }

        [Fact]
        public async Task RetrieveUsersAsync_ReturnsValid_ForValidUsernames() {
            List<string> usernames = ["arrow", "spartan"];
            List<string> validatedUsernames = ["arrow", "spartan"];
            List<User> users = [
                new User { Name = "John Diggle", Login = "spartan" },
                new User { Name = "Oliver Queen", Login = "arrow" }
            ];
            _ValidatorMock.Setup(v => v.ValidateUsernames(usernames, out It.Ref<Dictionary<string, string>>.IsAny))
                          .Returns(validatedUsernames)
                          .Callback((List<string> _, out Dictionary<string, string> errors) => {
                              errors = [];
                           });
            _ApiConnectorMock.Setup(a => a.RetrieveUsersAsync("arrow")).ReturnsAsync(users[1]);
            _ApiConnectorMock.Setup(a => a.RetrieveUsersAsync("spartan")).ReturnsAsync(users[0]);

            ApiUserResponse result = await _GitHubService.RetrieveUsersAsync(usernames);

            Assert.NotNull(result);
            Assert.Equal(2, result.Users.Count);
            Assert.Empty(result.Errors);
            Assert.Equal("spartan", result.Users[0].Login);
            Assert.Equal("arrow", result.Users[1].Login);
        }

        [Fact]
        public async Task RetrieveUsersAsync_ReturnsValid_ForValidAndDuplicateUsername() {
            List<string> usernames = ["overwatch", "Overwatch", "overWatch", "over watch"];
            List<string> validatedUsernames = ["overwatch"];
            User user = new() { Name = "Felicity Smoak", Login = "overwatch" };
            _ValidatorMock.Setup(v => v.ValidateUsernames(usernames, out It.Ref<Dictionary<string, string>>.IsAny))
                          .Returns(validatedUsernames)
                          .Callback((List<string> _, out Dictionary<string, string> errors) => {
                              errors = [];
                              errors.Add("Overwatch", "Duplicate username found.");
                              errors.Add("overWatch", "Duplicate username found.");
                              errors.Add("over watch", "Username is not in valid alphanumeric and hypen format.");
                          });
            _ApiConnectorMock.Setup(a => a.RetrieveUsersAsync("overwatch")).ReturnsAsync(user);

            ApiUserResponse result = await _GitHubService.RetrieveUsersAsync(usernames);

            Assert.NotNull(result);
            Assert.Single(result.Users);
            Assert.Equal(3, result.Errors.Count);
            Assert.Equal("Overwatch", result.Errors[0].Username);
            Assert.Equal("Duplicate username found.", result.Errors[0].Message);
            Assert.Equal("overWatch", result.Errors[1].Username);
            Assert.Equal("Duplicate username found.", result.Errors[1].Message);
            Assert.Equal("over watch", result.Errors[2].Username);
            Assert.Equal("Username is not in valid alphanumeric and hypen format.", result.Errors[2].Message);
        }
    }
}
