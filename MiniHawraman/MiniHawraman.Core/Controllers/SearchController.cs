using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public class SearchController : PublicBaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;

        public SearchController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService, episodeService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
        }

        public ActionResult Index(string query, int? count)
        {
            ViewBag.PageTitle = string.Format("{0} | {1}", "Search", this._siteService.Title);
            query = query.ToLower(CultureInfo.InvariantCulture);
            return View(this._episodeService.SearchEpisodes(query, 50).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string search)
        {
            return RedirectToAction("Index", new { query = search });
        }
    }
}
