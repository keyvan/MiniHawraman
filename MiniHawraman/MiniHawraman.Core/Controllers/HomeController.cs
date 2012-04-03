using System.Web.Mvc;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class HomeController : PublicBaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;

        public HomeController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService, episodeService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
        }

        public ActionResult Index()
        {
            return View(this._episodeService.GetRecentEpisodes(this._siteService.PageSize));
        }
    }
}
