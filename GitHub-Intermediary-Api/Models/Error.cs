namespace GitHub_Intermediary_Api.Models {
    public class Error {
        public string Username { get; set; }
        public string Message { get; set; }

        public Error() {            
            Username = ""; Message = "";
        }

        public Error(string username, string message) {
            Username = username; 
            Message = message;
        }
    }
}
