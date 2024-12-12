namespace GitHub_Intermediary_Api.Models {
    public class ApiUserResponse {
        public List<User> Users { get; set; }
        public List<Error> Errors { get; set; }

        public ApiUserResponse() {
            Users = []; Errors = [];
        }

        public ApiUserResponse(List<User> users, List<Error> errors) {
            Users = users; Errors = errors;
        }
    }
}
