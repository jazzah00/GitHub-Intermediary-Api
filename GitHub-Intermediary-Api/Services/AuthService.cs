using GitHub_Intermediary_Api.Interfaces;

namespace GitHub_Intermediary_Api.Services {
    public class AuthService : IAuthService {
        private static readonly Dictionary<string, DateTime> _Tokens = [];

        public string GenerateToken() {
            RefreshTokens();
            string token = Guid.NewGuid().ToString();
            _Tokens[token] = DateTime.UtcNow;
            return token;
        }

        public bool ValidateToken(string token) {
            RefreshTokens();
            if (_Tokens.ContainsKey(token)) {
                return true;
            }
            return false;
        }

        public bool ValidateTokenTest(string token) {
            _Tokens.Clear();
            return false;
        }

        private static void RefreshTokens() {
            foreach (var obj in _Tokens) {
                if (obj.Value.AddMinutes(5) < DateTime.UtcNow) {
                    _Tokens.Remove(obj.Key);
                }
            }
        }
    }
}
