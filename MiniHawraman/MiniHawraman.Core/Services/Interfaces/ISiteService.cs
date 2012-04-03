
using MiniHawraman.Core.Models;
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface ISiteService
    {
        string Title { get; set; }
        string Description { get; set; }
        int PageSize { get; set; }
        int AdminPageSize { get; set; }
        string Feed { get; }

        void SetSettings(SiteSettings settings);
        SiteSettings GetSettings();
        void SaveSettings();
    }
}
