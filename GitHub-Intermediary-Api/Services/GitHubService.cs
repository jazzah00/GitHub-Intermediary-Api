using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Models;

namespace GitHub_Intermediary_Api.Services {
    public class GitHubService(IApiConnector apiConnector, IValidator validator) : IGitHubService {
        private readonly IApiConnector _ApiConnector = apiConnector;
        private readonly IValidator _Validator = validator;

        public async Task<ApiUserResponse> RetrieveUsersAsync(List<string> usernames) {
            usernames = _Validator.ValidateUsernames(usernames, out Dictionary<string, string> errors);

            List<User> users = [];
            foreach (string username in usernames) {
                User? user = await _ApiConnector.RetrieveUsersAsync(username);
                if (user != null && !users.Contains(user)) users.Add(user);
                else errors.TryAdd(username, "User not found.");
            }

            return new ApiUserResponse {
                Users = [.. users.OrderBy(u => u.Name)],
                Errors = errors.Select(e => new Error { Username = e.Key, Message = e.Value }).ToList()
            };
        }
    }
}
