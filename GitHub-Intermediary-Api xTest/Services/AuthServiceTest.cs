using GitHub_Intermediary_Api.Services;

namespace GitHub_Intermediary_Api_xTest.Services {
    public class AuthServicesTest {
        private readonly AuthService _AuthService;

        public AuthServicesTest() {
            _AuthService = new AuthService();
        }

        [Fact]
        public void GenerateToken_ReturnValid() {
            string token1 = _AuthService.GenerateToken();
            string token2 = _AuthService.GenerateToken();

            Assert.False(string.IsNullOrEmpty(token1));
            Assert.False(string.IsNullOrEmpty(token2));
            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void ValidateToken_ReturnValid_ForValidToken() {
            string token = _AuthService.GenerateToken();

            bool result = _AuthService.ValidateToken(token);

            Assert.False(string.IsNullOrEmpty(token));
            Assert.True(result);
        }

        [Fact]
        public void ValidateToken_ReturnInvalid_ForInvalidToken() {
            string token = Guid.NewGuid().ToString();

            bool result = _AuthService.ValidateToken(token);

            Assert.False(string.IsNullOrEmpty(token));
            Assert.False(result);
        }

        [Fact]
        public void ValidateToken_ReturnInvalid_ForExpiredToken() {
            string token = _AuthService.GenerateToken();            

            // Uncomment below if you want to truely test the 5-min lifespan
            //Thread.Sleep((5 * 60 * 1000) + (10 * 1000));
            //bool result = _AuthService.ValidateToken(token);

            // Comment below if testing the above
            bool result = _AuthService.ValidateTokenTest(token);

            Assert.False(string.IsNullOrEmpty(token));
            Assert.False(result);
        }
    }
}