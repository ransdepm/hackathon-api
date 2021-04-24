

namespace Hackathon.Service.Configuration
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int Expiration { get; set; }
        public string ConnectionString { get; set; }
        public string SportsDataApiKey { get; set; }
    }
}
