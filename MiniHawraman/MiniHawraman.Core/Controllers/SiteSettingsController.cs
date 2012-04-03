using System.Web.Mvc;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    [Authorize()]
    public class SiteSettingsController : BaseController
    {
        private ISiteService _siteService;

        public SiteSettingsController(ISiteService siteService)
            : base(siteService)
        {
            this._siteService = siteService;
        }

        public ActionResult Index()
        {
            return View(this._siteService.GetSettings());
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(SiteSettings settings)
        {
            this._siteService.SetSettings(settings);

            return RedirectToAction("Index");
        }
    }
}
