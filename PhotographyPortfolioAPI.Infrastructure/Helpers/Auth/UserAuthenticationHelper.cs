using PhotographyPortfolioAPI.Core.Enums.Auth;
using System.Security.Cryptography;
using System.Text;

namespace PhotographyPortfolioAPI.Infrastructure.Helpers.Auth
{
    public class UserAuthenticationHelper
    {
        private const int DefaultOtpLength = 6;

        public static string GenerateOtp(int length = DefaultOtpLength)
        {
            var otpBuilder = new StringBuilder(length);
            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[1];
            while (otpBuilder.Length < length)
            {
                rng.GetBytes(buffer);
                otpBuilder.Append(buffer[0] % 10);
            }
            return otpBuilder.ToString();
        }

        public static int GetOtpExpiryInMinutes(UserAuthOtpType otpType) => 60;
    }
}
