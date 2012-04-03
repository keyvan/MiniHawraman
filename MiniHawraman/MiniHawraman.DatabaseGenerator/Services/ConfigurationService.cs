using System;
using MiniHawraman.DatabaseGenerator.Model;
using Raven.Client;

namespace MiniHawraman.DatabaseGenerator.Services
{
    class ConfigurationService
    {
        public void AddConfiguration(Configuration configuration)
        {
            configuration.DateAdded = DateTime.UtcNow;

            using (IDocumentSession session = Program.documentStore.OpenSession())
            {
                session.Store(configuration);
                session.SaveChanges();
            }
        }
    }
}
