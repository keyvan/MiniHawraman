using System.Configuration;
using System.Web.Mvc;
using MiniHawraman.Core.ActionResults;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class SubscriptionController : BaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;

        public SubscriptionController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
        }

        public RssResult RssRecent()
        {
            return new RssResult(this._siteService.GetSettings(),
                this._episodeService.GetRecentEpisodes(this._siteService.PageSize));
        }

        public RssResult RssFull()
        {
            return new RssResult(this._siteService.GetSettings(),
                this._episodeService.GetEpisodes());
        }

        public ActionResult Itunes()
        {
            return new RedirectResult(ConfigurationManager.AppSettings["iTunesSubscriptionUrl"], false);
        }

        public ActionResult Zune()
        {
            return new RedirectResult(ConfigurationManager.AppSettings["ZuneSubscriptionUrl"], false);
        }

        public ActionResult Facebook()
        {
            return new RedirectResult(ConfigurationManager.AppSettings["FacebookUrl"], false);
        }

        public ActionResult Twitter()
        {
            return new RedirectResult(ConfigurationManager.AppSettings["TwitterUrl"], false);
        }
    }
}
