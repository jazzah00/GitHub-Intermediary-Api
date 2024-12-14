namespace GitHub_Intermediary_Api.Interfaces {
    public interface IAuthService {
        string GenerateToken();        
        bool ValidateToken(string token);
    }
}
