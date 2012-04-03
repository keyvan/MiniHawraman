using System;
using System.Security.Cryptography;
using System.Text;
using MiniHawraman.DatabaseGenerator.Model;
using MiniHawraman.DatabaseGenerator.Services;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace MiniHawraman.DatabaseGenerator
{
    class Program
    {
        public static DocumentStore documentStore;

        static void Main(string[] args)
        {
            Console.Title = "Mini Hawraman RavenDB Database Creator";

            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            documentStore = new DocumentStore
            {
                ApiKey = parser.ConnectionStringOptions.ApiKey,
                Url = parser.ConnectionStringOptions.Url,
            };
            documentStore.Initialize();

            Console.WriteLine("Please enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Please enter your email:");
            string email = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();

            AddUser(name, email, password);
            AddConfig();

            Console.WriteLine("Done!");

            Console.ReadLine();
        }

        private static void AddConfig()
        {
            Configuration config = new Configuration();
            config.Name = "SiteSettings";
            config.Config = "<SiteSettings><Title>Keyvan.FM</Title><Description>The podcast channel of Keyvan Nayyeri about software development, computer science, and technology.</Description><PageSize>10</PageSize><AdminPageSize>20</AdminPageSize></SiteSettings>";
            config.DateAdded = DateTime.UtcNow;

            ConfigurationService service = new ConfigurationService();
            service.AddConfiguration(config);
        }

        private static void AddUser(string name, string email, string password)
        {
            User user = new User();
            user.DisplayName = name;
            user.Username = "admin";
            user.Email = email;
            user.Password = HashString(password);

            UserService service = new UserService();
            service.AddUser(user);
        }

        public static string HashString(string text)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
