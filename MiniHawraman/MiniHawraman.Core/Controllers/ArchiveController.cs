using System.Web.Mvc;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class ArchiveController : PublicBaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;

        public ArchiveController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService, episodeService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
        }

        public ActionResult Index()
        {
            ViewBag.PageTitle = string.Format("{0} | {1}", "Archive", this._siteService.Title);
            return View(this._episodeService.GetEpisodes());
        }
    }
}
