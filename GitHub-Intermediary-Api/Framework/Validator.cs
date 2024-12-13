using System.Text.RegularExpressions;

namespace GitHub_Intermediary_Api.Framework {
    public class Validator() {
        private readonly int ValidTokenTime = 60;

        public List<string> ValidateUsernames(List<string> usernames, out Dictionary<string, string> errors) {
            List<string> validUsernames = []; errors = [];
            Regex regex = new(@"^[a-zA-Z0-9-]+$");
            foreach (string username in usernames) {
                if (regex.IsMatch(username)) {
                    if (!validUsernames.Contains(username.ToLower())) validUsernames.Add(username.ToLower());
                    else errors.TryAdd(username.ToLower(), "Duplicate username found.");
                } else errors.TryAdd(username.ToLower(), "Username is not in valid alphanumeric and hypen format.");
            }
            return validUsernames;
        }

        public bool ValidateAuthenticationToken(ref Dictionary<string, DateTime> currentTokens, string authToken) {
            if (currentTokens.Count > 0 && currentTokens.TryGetValue(authToken, out DateTime creationDate)) {
                TimeSpan difference = DateTime.UtcNow - creationDate;
                if (Math.Abs(difference.TotalSeconds) <= ValidTokenTime) return true;
                currentTokens.Remove(authToken);
            }
            return false;
        }
    }
}
