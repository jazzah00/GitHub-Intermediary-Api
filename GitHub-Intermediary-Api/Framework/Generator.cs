namespace GitHub_Intermediary_Api.Framework {
    public class Generator() {
        public string GenerateRandomAccessToken() {
            Random random = new(); char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^*?".ToCharArray();
            char[] accessToken = new char[64];
            for (int i = 0; i < 64; i++) accessToken[i] = chars[random.Next(chars.Length)];
            return new string(accessToken);
        }
    }
}
