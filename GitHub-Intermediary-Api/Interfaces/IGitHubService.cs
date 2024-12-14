using GitHub_Intermediary_Api.Models;

namespace GitHub_Intermediary_Api.Interfaces {
    public interface IGitHubService {
        Task<ApiUserResponse> RetrieveUsersAsync(List<string> usernames);
    }
}
