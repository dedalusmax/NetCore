namespace NetCore.Business.Authentication
{
    public class AuthenticationInfo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
