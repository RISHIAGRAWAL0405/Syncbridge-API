namespace PhotographyPortfolioAPI.Contracts.Models.Auth
{
    public class UserPermissionsData
    {
        public Guid permission_id { get; set; }
        public Guid role_id { get; set; }
        public string permission_code { get; set; }
        public string permission_name { get; set; }
        public string module_name { get; set; }
        public bool can_view { get; set; }
        public bool can_add { get; set; }
        public bool can_update { get; set; }
        public bool can_delete { get; set; }
        public bool can_export { get; set; }
    }
}
