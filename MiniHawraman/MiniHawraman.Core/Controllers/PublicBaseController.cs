using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Controllers
{
    public abstract class PublicBaseController : BaseController
    {
        private IEpisodeService _episodeService;

        public PublicBaseController(ISiteService siteService, IEpisodeService episodeService)
            : base(siteService)
        {
            this._episodeService = episodeService;
        }
    }
}
