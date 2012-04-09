using System;
using System.Configuration;
using System.IO;
using System.Xml;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Services.Implementations
{
    public class SiteService : ISiteService
    {
        private IConfigurationService _configurationService;

        public SiteService(IConfigurationService configurationService)
        {
            this._configurationService = configurationService;
            this.LoadSettings();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PageSize { get; set; }

        public int AdminPageSize { get; set; }

        public string Feed
        {
            get
            {
                return string.Format("{0}rss", ConfigurationManager.AppSettings["DomainName"]);
            }
        }

        public void SetSettings(SiteSettings settings)
        {
            this.Title = settings.Title;
            this.Description = settings.Description;
            this.PageSize = settings.PageSize;
            this.AdminPageSize = settings.AdminPageSize;

            this.SaveSettings();
        }

        public SiteSettings GetSettings()
        {
            SiteSettings settings = new SiteSettings();
            settings.Title = this.Title;
            settings.Description = this.Description;
            settings.PageSize = this.PageSize;
            settings.AdminPageSize = this.AdminPageSize;

            return settings;
        }

        public void SaveSettings()
        {
            if (this != null)
            {
                string configString = string.Empty;
                StringWriter sw = new StringWriter();

                using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.WriteStartElement("SiteSettings");

                    if (!string.IsNullOrEmpty(this.Title))
                        writer.WriteElementString("Title", this.Title);

                    if (!string.IsNullOrEmpty(this.Description))
                        writer.WriteElementString("Description", this.Description);

                    writer.WriteElementString("PageSize", this.PageSize.ToString());
                    writer.WriteElementString("AdminPageSize", this.AdminPageSize.ToString());


                    writer.WriteEndElement();

                    writer.Flush();
                    configString = sw.ToString();
                    sw.Close();
                    writer.Close();
                }

                MiniHawraman.Core.Models.Configuration config = this._configurationService.GetConfiguration("SiteSettings");

                if (config == null)
                {
                    config = new MiniHawraman.Core.Models.Configuration();
                    config.Name = "SiteSettings";
                    config.Config = configString;
                    config.DateAdded = DateTime.Now;

                    this._configurationService.AddConfiguration(config);
                }
                else
                {
                    config.Config = configString;
                    this._configurationService.EditConfiguration(config);
                }
            }
        }

        private void LoadSettings()
        {
            MiniHawraman.Core.Models.Configuration config = this._configurationService.GetConfiguration("SiteSettings");

            if (config != null)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(config.Config);

                this.Title = GetConfigSection(xml, "SiteSettings/Title").InnerText ?? string.Empty;
                this.Description = GetConfigSection(xml, "SiteSettings/Description").InnerText ?? string.Empty;
                this.PageSize = int.Parse(GetConfigSection(xml, "SiteSettings/PageSize").InnerText ?? "0");
                this.AdminPageSize = int.Parse(GetConfigSection(xml, "SiteSettings/AdminPageSize").InnerText ?? "0");
            }
        }

        public XmlNode GetConfigSection(XmlDocument xml, string nodePath)
        {
            return xml.SelectSingleNode(nodePath);
        }
    }
}
