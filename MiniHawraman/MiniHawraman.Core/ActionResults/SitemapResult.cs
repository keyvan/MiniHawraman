using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Xml;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;

namespace MiniHawraman.Core.ActionResults
{
    public class SitemapResult : ActionResult
    {
        private IEnumerable<Episode> _episodes;

        public SitemapResult(IEnumerable<Episode> episodes)
        {
            this._episodes = episodes;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                string videoSitemapUri = "http://www.google.com/schemas/sitemap-video/1.1";

                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "video", null, videoSitemapUri);

                // Home
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", ConfigurationManager.AppSettings["DomainName"]);
                writer.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                writer.WriteElementString("changefreq", "daily");
                writer.WriteElementString("priority", "1.0");
                writer.WriteEndElement();

                // Episodes
                foreach (Episode episode in this._episodes)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", EpisodeUtil.GetEpisodeUrl(episode.Slug));

                    if (episode.UpdatedOn.HasValue)
                        writer.WriteElementString("lastmod", episode.UpdatedOn.Value.ToString("yyyy-MM-dd"));

                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.5");

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
        }
    }
}
