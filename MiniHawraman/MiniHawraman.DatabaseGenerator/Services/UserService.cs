using System;
using MiniHawraman.DatabaseGenerator.Model;
using Raven.Client;

namespace MiniHawraman.DatabaseGenerator.Services
{
    public class UserService
    {
        public void AddUser(User user)
        {
            user.DateAdded = DateTime.UtcNow;

            using (IDocumentSession session = Program.documentStore.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}
