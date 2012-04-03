using System.Configuration;

namespace MiniHawraman.Core.Components
{
    public static class EpisodeUtil
    {
        public static string GetEpisodeUrl(string slug)
        {
            return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainName"], slug);
        }
    }
}
