using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Models;
using Newtonsoft.Json;

namespace GitHub_Intermediary_Api.Services {
    public class ApiConnector : IApiConnector {
        private readonly Dictionary<string, string> GitHubHeaders = new() {
            { "Accept", "application/vnd.github+json" },
            { "X-GitHub-Api-Version", "2022-11-28" },
            { "User-Agent", "CSharp-App" }
        };

        private async Task<T?> GetAsync<T>(string url) where T : class {
            try {
                using (HttpClient client = new()) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    foreach (var obj in GitHubHeaders) client.DefaultRequestHeaders.Add(obj.Key, obj.Value);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode) {
                        if (response.Content != null) {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<T>(jsonResponse);
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error Message: {ex.Message}");
            }
            return null;
        }

        public async Task<User?> RetrieveUsersAsync(string username) {
            string url = "https://api.github.com/users/";
            return await GetAsync<User>($"{url}{username}");
        }
    }
}
