using GitHub_Intermediary_Api.Interfaces;
using System.Text.RegularExpressions;

namespace GitHub_Intermediary_Api.Services {
    public class Validator : IValidator {
        public List<string> ValidateUsernames(List<string> usernames, out Dictionary<string, string> errors) {
            List<string> validUsernames = []; errors = [];
            Regex regex = new(@"^[a-zA-Z0-9-]+$");
            foreach (string username in usernames) {
                if (regex.IsMatch(username)) {
                    if (!validUsernames.Where(u => u.ToLower().Equals(username.ToLower())).Any()) validUsernames.Add(username);
                    else errors.TryAdd(username, "Duplicate username found.");
                } else errors.TryAdd(username, "Username is not in valid alphanumeric and hypen format.");
            }
            return validUsernames;
        }
    }
}
