using GitHub_Intermediary_Api.Models;
using GitHub_Intermediary_Api.Services;
using Moq;

namespace GitHub_Intermediary_Api_xTest.Services {
    public class ApiConnectorTest {
        private readonly Mock<HttpMessageHandler> _HttpMessageHandlerMock;
        private readonly HttpClient _HttpClient;
        private readonly ApiConnector _ApiConnector;

        public ApiConnectorTest() { 
            _HttpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _HttpClient = new HttpClient(_HttpMessageHandlerMock.Object);
            _ApiConnector = new ApiConnector();
        }

        [Fact]
        public async Task RetrieveUsersAync_ReturnValid_ForValidUsername() {
            User? user = await _ApiConnector.RetrieveUsersAsync("octacat");

            Assert.NotNull(user);
            Assert.Equal("octacat", user.Login);
        }

        [Fact]
        public async Task RetrieveUsersAsync_ReturnInvalid_ForValidUsername() {
            User? user = await _ApiConnector.RetrieveUsersAsync("petepenguin");

            Assert.Null(user);
        }
    }
}
