using System.Web.Mvc;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public abstract class BaseController : Controller
    {
        private ISiteService _siteService;

        public BaseController(ISiteService blogService)
        {
            this._siteService = blogService;

            ViewBag.PageTitle = this._siteService.Title;
            ViewBag.SiteDescription = this._siteService.Description;
            ViewBag.SiteFeed = this._siteService.Feed;
        }
    }
}
