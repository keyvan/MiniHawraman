using System;
using System.Collections.Generic;
using System.Linq;
using MiniHawraman.Core.Components;
using MiniHawraman.Core.Models;
using MiniHawraman.Core.Services.Interfaces;
using Raven.Client;

namespace MiniHawraman.Core.Services.Implementations
{
    public class UserService : IUserService
    {
        public User GetUser(int id)
        {
            User user = new User();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                user = session.Query<User>().Where(u => u.Id == id).SingleOrDefault();
            }

            return user;
        }

        public User GetUser(string username)
        {
            User user = new User();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                user = session.Query<User>().Where(u => u.Username == username).SingleOrDefault();
            }

            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                users = session.Query<User>().ToList<User>();
            }

            return users;
        }

        public void AddUser(User user)
        {
            user.DateAdded = DateTime.UtcNow;

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
            }
        }

        public void EditUser(User user)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                User editedUser = session.Load<User>
                    (string.Format("users/{0}", user.Id.ToString()));

                editedUser.Email = user.Email;
                editedUser.DisplayName = user.DisplayName;
                editedUser.Biography = user.Biography;

                session.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                User user = session.Load<User>
                    (string.Format("users/{0}", id.ToString()));

                session.Delete(user);
                session.SaveChanges();
            }
        }

        public bool ValidateUser(string username, string password)
        {
            User user = new User();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                user = session.Query<User>().Where(u => u.Username == username).SingleOrDefault();
            }

            if (user == null)
                return false;
            if (user.Password == Util.HashString(password))
                return true;
            else
                return false;
        }

        public void SetValidationCode(string username, string code)
        {
            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                User user = session.Query<User>().Where(u => u.Username == username).SingleOrDefault();
                User editedUser = session.Load<User>(string.Format("users/{0}", user.Id));

                editedUser.VerificationCode = code;

                session.SaveChanges();
            }
        }

        public string GetValidationCode(string username)
        {
            User user = new User();

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                user = session.Query<User>().Where(u => u.Username == username).SingleOrDefault();
            }

            return user.VerificationCode;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool result = false;

            using (IDocumentSession session = HawramanApplication.RavenDbDocumentStore.OpenSession())
            {
                User editedUser = session.Query<User>().Where(u => u.Username == username).SingleOrDefault();

                if (editedUser.Password == Util.HashString(oldPassword))
                {
                    editedUser.Password = Util.HashString(newPassword);
                    session.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}
