using MiniHawraman.Core.ActionResults;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class SitemapController : PublicBaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;

        public SitemapController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService, episodeService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
        }

        public SitemapResult Sitemap()
        {
            return new SitemapResult(this._episodeService.GetEpisodes());
        }
    }
}
