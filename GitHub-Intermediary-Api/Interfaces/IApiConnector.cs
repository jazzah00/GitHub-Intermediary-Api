using GitHub_Intermediary_Api.Models;

namespace GitHub_Intermediary_Api.Interfaces {
    public interface IApiConnector {
        Task<User?> RetrieveUsersAsync(string username);
    }
}
