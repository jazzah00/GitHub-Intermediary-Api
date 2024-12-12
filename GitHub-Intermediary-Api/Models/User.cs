namespace GitHub_Intermediary_Api.Models {
    public class User {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Company { get; set; }
        public int Followers { get; set; }
        public int Public_Repos { get; set; }
        public int Average_Followers_Per_Repository => Public_Repos == 0 ? 0 : Followers / Public_Repos;

        public User() {
            Name = ""; Login = ""; Company = "";
            Followers = 0; Public_Repos = 0;
        }

        public User(string name, string login, string company, int followers, int public_repos) {
            Name = name;
            Login = login;
            Company = company;
            Followers = followers;
            Public_Repos = public_repos;
        }

        public bool IsValid() => Name != null && !Name.Equals("") && Login != null && !Login.Equals("");
    }
}
