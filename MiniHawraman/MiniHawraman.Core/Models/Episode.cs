using System;
using System.Collections.Generic;

namespace MiniHawraman.Core.Models
{
    public class Episode
    {
        public int Id { get; set; }

        public int EpisodeNumber { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string Guests { get; set; }

        public string GuestBiography { get; set; }

        public string Slug { get; set; }

        public DateTime DateAdded { get; set; }

        public string Duration { get; set; }

        public int Views { get; set; }

        public Nullable<DateTime> UpdatedOn { get; set; }

        public bool IsPublished { get; set; }

        public string DownloadLink { get; set; }

        public int DownloadSize { get; set; }

        public string Links { get; set; }

        public Dictionary<string, string> EpisodeLinks
        {
            get
            {
                Dictionary<string, string> links = new Dictionary<string, string>();

                try
                {
                    foreach (string line in this.Links.Split('\n'))
                    {
                        string[] temp = line.Split('|');
                        links.Add(temp[0].Trim(), temp[1].Trim());
                    }
                }
                catch
                {
                }
                return links;
            }
        }
    }
}
