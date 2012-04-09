using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;

namespace MiniHawraman.Core.ActionResults
{
    public class RssResult : ActionResult
    {
        private SiteSettings _siteSettings;
        private IEnumerable<Episode> _episodes;

        public RssResult(SiteSettings siteSettings, IEnumerable<Episode> episodes)
        {
            this._siteSettings = siteSettings;
            this._episodes = episodes;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string authorName = ConfigurationManager.AppSettings["RssAuthorName"];
            string authorEmail = ConfigurationManager.AppSettings["RssAuthorEmail"];

            context.HttpContext.Response.ContentType = "application/rss+xml";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output, settings))
            {
                string itunesUri = "http://www.itunes.com/dtds/podcast-1.0.dtd";

                // Start document
                writer.WriteStartDocument();

                // Start rss
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("xmlns", "itunes", null, itunesUri);
                writer.WriteAttributeString("version", "2.0");

                // Start channel
                writer.WriteStartElement("channel");

                writer.WriteElementString("title", this._siteSettings.Title);
                writer.WriteElementString("description", this._siteSettings.Description);
                writer.WriteElementString("link", ConfigurationManager.AppSettings["DomainName"]);
                writer.WriteElementString("language", "en-us");
                writer.WriteElementString("copyright", string.Format("{0} {1}. All rights reserved.", DateTime.UtcNow.Year, authorName));
                writer.WriteElementString("lastBuildDate", DateTime.UtcNow.ToString("r"));
                writer.WriteElementString("pubDate", DateTime.UtcNow.ToString("r"));
                writer.WriteElementString("webMaster", authorEmail);

                // Start image
                writer.WriteStartElement("image");

                writer.WriteElementString("url",
                    ConfigurationManager.AppSettings["DomainName"] + "content/images/feed-logo.png");
                writer.WriteElementString("title", this._siteSettings.Title);
                writer.WriteElementString("link", ConfigurationManager.AppSettings["DomainName"]);

                writer.WriteElementString("width", "300");
                writer.WriteElementString("height", "300");
                writer.WriteElementString("description", this._siteSettings.Description);


                // End image
                writer.WriteEndElement();


                writer.WriteElementString("author", itunesUri, authorName);
                writer.WriteElementString("subtitle", itunesUri, this._siteSettings.Description);
                writer.WriteElementString("summary", itunesUri, this._siteSettings.Description);

                // Start itunes:owner
                writer.WriteStartElement("owner", itunesUri);

                writer.WriteElementString("name", itunesUri, authorName);
                writer.WriteElementString("email", itunesUri, authorEmail);

                // End  itunes:owner
                writer.WriteEndElement();

                writer.WriteElementString("explicit", itunesUri, "No");

                // Start itunes:image
                writer.WriteStartElement("image", itunesUri);

                writer.WriteAttributeString("href",
                    ConfigurationManager.AppSettings["DomainName"] + "content/images/feed-logo.png");

                // End itunes:image
                writer.WriteEndElement();

                // First category
                // Start itunes:category
                writer.WriteStartElement("category", itunesUri);
                writer.WriteAttributeString("text", ConfigurationManager.AppSettings["iTunesCategory"]);

                // Start itunes:category
                writer.WriteStartElement("category", itunesUri);
                writer.WriteAttributeString("text", ConfigurationManager.AppSettings["iTunesSubCategory"]);
                // End itunes:category
                writer.WriteEndElement();

                // End itunes:category
                writer.WriteEndElement();


                // Write episodes
                foreach (Episode episode in this._episodes)
                {
                    // Start item
                    writer.WriteStartElement("item");

                    writer.WriteElementString("title", episode.Title);
                    writer.WriteElementString("link", EpisodeUtil.GetEpisodeUrl(episode.Slug));

                    // Guid
                    writer.WriteStartElement("guid");
                    writer.WriteAttributeString("isPermaLink", "true");
                    writer.WriteString(EpisodeUtil.GetEpisodeUrl(episode.Slug));
                    writer.WriteEndElement();


                    writer.WriteElementString("description", episode.Summary);

                    // Start enclosure 
                    writer.WriteStartElement("enclosure");

                    writer.WriteAttributeString("url", episode.DownloadLink);

                    writer.WriteAttributeString("length", episode.DownloadSize.ToString());
                    writer.WriteAttributeString("type", "audio/mpeg");


                    // End enclosure
                    writer.WriteEndElement();

                    writer.WriteElementString("pubDate", episode.DateAdded.ToString("r"));

                    writer.WriteElementString("author", itunesUri, authorName);
                    writer.WriteElementString("explicit", itunesUri, "No");
                    writer.WriteElementString("subtitle", itunesUri, episode.Summary);
                    writer.WriteElementString("summary", itunesUri, episode.Summary);
                    writer.WriteElementString("duration", itunesUri, episode.Duration);
                    writer.WriteElementString("keywords", itunesUri, "");

                    // End item
                    writer.WriteEndElement();
                }


                // End channel
                writer.WriteEndElement();

                // End rss
                writer.WriteEndElement();

                // End document
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();
            }
        }
    }
}
