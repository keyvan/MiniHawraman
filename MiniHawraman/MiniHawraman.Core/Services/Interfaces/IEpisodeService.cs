using System.Collections.Generic;
using MiniHawraman.Core.Models;
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface IEpisodeService
    {
        Episode GetEpisode(int id);
        Episode GetEpisode(string slug);
        IEnumerable<Episode> GetEpisodes();
        IEnumerable<Episode> GetEpisodes(int pageSize, int pageIndex, bool? published = false);
        IEnumerable<Episode> GetRecentEpisodes(int count);
        IEnumerable<Episode> SearchEpisodes(string query, int count);
        int AddEpisode(Episode episode);
        void EditEpisode(Episode episode, bool? updateDate);
        void DeleteEpisode(int id);
        void IncrementViews(int id);
    }
}
