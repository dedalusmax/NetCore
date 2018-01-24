using Microsoft.Extensions.Configuration;

namespace NetCore.Business.Models
{
    public class AppOptions
    {
        public AppOptions(IConfiguration configuration)
        {
            BaseUrl = configuration["BaseUrl"] ?? string.Empty;
        }

        public string BaseUrl { get; set; }
    }
}
