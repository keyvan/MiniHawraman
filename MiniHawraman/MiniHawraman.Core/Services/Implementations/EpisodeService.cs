using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;
using Raven.Client;

namespace MiniHawraman.Core.Services.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        public Episode GetEpisode(int id)
        {
            Episode episode = new Episode();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                episode = session.Query<Episode>().Where(e => e.Id == id).SingleOrDefault();
            }

            return episode;
        }

        public Episode GetEpisode(string slug)
        {
            Episode episode = new Episode();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                episode = session.Query<Episode>().Where(e => e.Slug == slug).SingleOrDefault();
            }

            return episode;
        }

        public IEnumerable<Episode> GetEpisodes()
        {
            List<Episode> result = new List<Episode>();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                List<Episode> episodes = session.Query<Episode>().OrderByDescending(e => e.DateAdded).ToList();

                foreach (Episode episode in episodes)
                {
                    result.Add(episode);
                }
            }

            return result;
        }

        public IEnumerable<Episode> GetEpisodes(int pageSize, int pageIndex, bool? published = false)
        {
            List<Episode> result = new List<Episode>();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                List<Episode> episodes = session.Query<Episode>().OrderByDescending(e => e.DateAdded).ToList<Episode>();

                if (published.Value)
                    episodes = session.Query<Episode>().Where(p => p.IsPublished).OrderByDescending(e => e.DateAdded).ToList<Episode>();

                int totalRecordCount = episodes.Count();

                result = episodes.Skip(pageSize * pageIndex).Take(pageSize).ToList<Episode>();
            }

            return result;
        }

        public IEnumerable<Episode> GetRecentEpisodes(int count)
        {
            List<Episode> result = new List<Episode>();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                result = session.Query<Episode>().OrderByDescending(e => e.DateAdded)
                    .Where(e => e.IsPublished).Take(count).ToList();
            }

            return result;
        }

        public IEnumerable<Episode> SearchEpisodes(string query, int count)
        {
            List<Episode> result = new List<Episode>();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                List<Episode> episodes = session.Query<Episode>().OrderByDescending(e => e.DateAdded).ToList<Episode>();

                string[] keywords = query.Split(' ');

                // Add your own
                List<string> keywordsToFilter = new List<string>() { "the", "in", "on", "for", "at", "of", "a", "an", "they" };

                for (int i = 0; i < keywords.Length; i++)
                {
                    string keyword = keywords[i].ToLower(CultureInfo.InvariantCulture).Trim();

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        if (!keywordsToFilter.Contains(keyword))
                            episodes = (
                                from e in episodes
                                where (e.Title.ToLower(CultureInfo.InvariantCulture).Contains(keyword) ||
                                e.Description.ToLower(CultureInfo.InvariantCulture).Contains(keyword) ||
                                e.Summary.ToLower(CultureInfo.InvariantCulture).Contains(keyword))
                                select e
                                ).ToList();
                    }
                }

                result = episodes;
            }

            return result.Take(count);
        }

        public int AddEpisode(Episode episode)
        {
            int id = 0;

            episode.DateAdded = DateTime.UtcNow;

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                episode.UpdatedOn = episode.DateAdded;

                episode.Title = episode.Title.TrimEnd();
                episode.Title = episode.Title.Replace("&ndash;", "-");
                episode.Description = episode.Description.Replace("&ndash;", "-");
                episode.Description = episode.Description.Replace("<P>", "<p>");
                episode.Description = episode.Description.Replace("</P>", "</p>");
                episode.Description = episode.Description.Replace("<br>", "<br />");
                episode.Description = episode.Description.Replace("<p></p>", "");

                episode.Slug = GetSlug(episode.Title);

                while (GetEpisodes().Where(e => e.Slug == episode.Slug).SingleOrDefault() != null)
                {
                    episode.Slug += string.Format("-{0}", episode.DateAdded.Year.ToString());
                }

                episode.EpisodeNumber = GetEpisodes().Count() + 1;

                session.Store(episode);
                session.SaveChanges();

                id = episode.Id;
            }

            return id;
        }

        public void EditEpisode(Episode episode, bool? updateDate)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                Episode editedEpisode = session.Load<Episode>
                    (string.Format("episodes/{0}", episode.Id.ToString()));

                editedEpisode.Title = episode.Title;
                editedEpisode.Description = episode.Description;
                editedEpisode.IsPublished = episode.IsPublished;
                editedEpisode.Links = episode.Links;
                editedEpisode.Guests = episode.Guests;
                editedEpisode.Summary = episode.Summary;
                editedEpisode.Duration = episode.Duration;
                if (updateDate.Value)
                    editedEpisode.DateAdded = DateTime.UtcNow;
                editedEpisode.UpdatedOn = DateTime.UtcNow;
                editedEpisode.GuestBiography = episode.GuestBiography;
                editedEpisode.DownloadLink = episode.DownloadLink;
                editedEpisode.DownloadSize = episode.DownloadSize;

                session.SaveChanges();
            }
        }

        public void DeleteEpisode(int id)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                Episode episode = session.Load<Episode>
                     (string.Format("episodes/{0}", id.ToString()));

                session.Delete(episode);
                session.SaveChanges();
            }
        }

        public void IncrementViews(int id)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                Episode editedEpisode = session.Load<Episode>
                    (string.Format("episodes/{0}", id.ToString()));

                editedEpisode.Views++;

                session.SaveChanges();
            }
        }

        private string GetSlug(string title)
        {
            string result = title.ToLower();

            foreach (Char ch in result)
            {
                if (ch == ' ')
                    continue;
                if (!Char.IsLetterOrDigit(ch) && ch != '.')
                    result = result.Replace(ch.ToString(), string.Empty);
            }

            result = Util.RemoveSpaceSequences(result);

            result = result.TrimEnd();
            result = result.Replace("-", "");
            result = result.Replace(" ", "-");
            result = result.Replace(".", "-");
            if (result.StartsWith("-"))
                result = result.Remove(0, 1);
            result = result.Replace("---", "-");
            result = result.Replace("--", "-");
            result = result.Trim();

            return result;
        }
    }
}
