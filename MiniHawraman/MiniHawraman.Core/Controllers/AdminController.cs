using System.Web.Mvc;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    [Authorize()]
    public class AdminController : BaseController
    {
        private ISiteService _siteService;

        public AdminController(ISiteService blogService)
            : base(blogService)
        {
            this._siteService = blogService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
