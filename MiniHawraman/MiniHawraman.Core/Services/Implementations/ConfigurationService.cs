using System;
using System.Linq;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;
using Raven.Client;

namespace MiniHawraman.Core.Services.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        public Configuration GetConfiguration(int id)
        {
            Configuration config = new Configuration();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                config = session.Query<Configuration>().Where(c => c.Id == id).SingleOrDefault();
            }

            return config;
        }

        public Configuration GetConfiguration(string name)
        {
            Configuration config = new Configuration();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                config = session.Query<Configuration>().Where(c => c.Name == name).SingleOrDefault();

            }

            return config;
        }

        public void AddConfiguration(Configuration configuration)
        {
            configuration.DateAdded = DateTime.UtcNow;

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                session.Store(configuration);
                session.SaveChanges();
            }
        }

        public void EditConfiguration(Configuration configuration)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                Configuration config = session.Load<Configuration>
                    (string.Format("configurations/{0}", configuration.Id.ToString()));
                config.Config = configuration.Config;

                session.SaveChanges();
            }
        }

        public void DeleteConfiguration(int id)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                Configuration config = session.Load<Configuration>
                    (string.Format("configurations/{0}", id.ToString()));

                session.Delete(config);
                session.SaveChanges();
            }
        }
    }
}
