using GitHub_Intermediary_Api.Services;

namespace GitHub_Intermediary_Api_xTest.Services {
    public class ValidatorTest {
        private readonly Validator _Validator;

        public ValidatorTest() {
            _Validator = new Validator();
        }

        [Fact]
        public void ValidateUsernames_ReturnValid() {
            List<string> usernames = ["octacat", "octadog", "octapus", "octanaut"];

            List<string> results = _Validator.ValidateUsernames(usernames, out Dictionary<string, string> errors);

            Assert.Equal(4, results.Count);
            Assert.Empty(errors);
            Assert.Contains("octacat", results);
            Assert.Contains("octadog", results);
            Assert.Contains("octapus", results);
            Assert.Contains("octanaut", results);
        }

        [Fact]
        public void ValidateUsernames_ReturnValidAndInvalid() {
            List<string> usernames = ["octacat", "oct@dog", "octa pus", "octanaut"];

            List<string> results = _Validator.ValidateUsernames(usernames, out Dictionary<string, string> errors);

            Assert.Equal(2, results.Count);
            Assert.Equal(2, errors.Count);
            Assert.Contains("octacat", results);
            Assert.Contains("oct@dog", errors);
            Assert.Contains("octa pus", errors);
            Assert.Contains("octanaut", results);
        }

        [Fact]
        public void ValidateUsernames_ReturnInvalid() {
            List<string> usernames = ["oct@dog", "octa pus"];

            List<string> results = _Validator.ValidateUsernames(usernames, out Dictionary<string, string> errors);

            Assert.Empty(results);
            Assert.Equal(2, errors.Count);
            Assert.Contains("oct@dog", errors);
            Assert.Contains("octa pus", errors);
            Assert.Equal("Username is not in valid alphanumeric and hypen format.", errors["oct@dog"]);
        }

        [Fact]
        public void ValidateUsernames_ReturnValidAndDuplicate() {
            List<string> usernames = ["octacat", "Octacat", "octaCat"];

            List<string> results = _Validator.ValidateUsernames(usernames, out Dictionary<string, string> errors);

            Assert.Single(results);
            Assert.Equal(2, errors.Count);
            Assert.Contains("octacat", results);
            Assert.Contains("Octacat", errors);
            Assert.Contains("octaCat", errors);
            Assert.Equal("Duplicate username found.", errors["Octacat"]);
        }
    }
}