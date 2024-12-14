namespace GitHub_Intermediary_Api.Interfaces {
    public interface IValidator {
        List<string> ValidateUsernames(List<string> usernames, out Dictionary<string, string> errors);
    }
}
