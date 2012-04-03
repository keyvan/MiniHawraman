using System.Configuration;
using System.Web.Mvc;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;
using System.Web.UI.HtmlControls;

namespace MiniHawraman.Core.Controllers
{
    public class EpisodeController : PublicBaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;
        private IUserService _userService;

        public EpisodeController(ISiteService siteService, IEpisodeService episodeService, IUserService userService)
            : base(siteService, episodeService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
            this._userService = userService;
        }

        public ActionResult Permalink(string slug)
        {
            Episode episode = this._episodeService.GetEpisode(slug);

            if (episode != null)
            {
                if (!string.IsNullOrEmpty(this._siteService.Title))
                    ViewBag.PageTitle = string.Format("{0} | {1}", episode.Title, this._siteService.Title);
                ViewBag.SiteDescription = episode.Summary;

                this._episodeService.IncrementViews(episode.Id);

                return View(episode);
            }
            else
                return RedirectPermanent(ConfigurationManager.AppSettings["DomainName"]);
        }
    }
}
