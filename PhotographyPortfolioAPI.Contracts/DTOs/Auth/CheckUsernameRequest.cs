namespace PhotographyPortfolioAPI.Contracts.DTOs.Auth
{
    public class CheckUsernameRequest
    {
        public string username { get; set; }
    }

    public class CheckUsernameResponse
    {
        public bool exists { get; set; }
        public string message { get; set; }
    }
}
