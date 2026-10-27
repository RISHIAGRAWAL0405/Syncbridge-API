namespace PhotographyPortfolioAPI.Helpers
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var key = Convert.FromBase64String(storedSalt);
            using var hmac = new System.Security.Cryptography.HMACSHA512(key);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}
