using System.Collections.Generic;
using System.Web.Mvc;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    [Authorize()]
    public class EpisodeAdminController : BaseController
    {
        private ISiteService _siteService;
        private IEpisodeService _episodeService;
        private IUserService _userService;

        public EpisodeAdminController(ISiteService siteService, IEpisodeService episodeService, IUserService userService)
            : base(siteService)
        {
            this._siteService = siteService;
            this._episodeService = episodeService;
            this._userService = userService;
        }

        public ActionResult Index(int? episodeId)
        {
            int index = 1;
            if (episodeId != null)
                index = (int)episodeId;

            IEnumerable<Episode> episodes = this._episodeService.GetEpisodes(this._siteService.AdminPageSize, index - 1);
            ViewData["PageIndex"] = index;
            ViewData["PageSize"] = this._siteService.AdminPageSize;

            return View(episodes);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Episode episode)
        {
            episode.DownloadSize = Util.GetFileLength(episode.DownloadLink);
            int episodeId = this._episodeService.AddEpisode(episode);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int episodeId)
        {
            return View(this._episodeService.GetEpisode(episodeId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int episodeId, Episode episode, bool updateDate)
        {
            episode.Id = episodeId;
            episode.DownloadSize = Util.GetFileLength(episode.DownloadLink);
            this._episodeService.EditEpisode(episode, updateDate);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int episodeId)
        {
            this._episodeService.DeleteEpisode(episodeId);

            return RedirectToAction("Index");
        }
    }
}
