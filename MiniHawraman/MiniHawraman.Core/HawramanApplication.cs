using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MiniHawraman.Core.ControllerFactories;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace MiniHawraman.Core
{
    public class HawramanApplication : HttpApplication
    {
        public static IDocumentStore RavenDbDocumentStore { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterAdminRoutes(routes);
            RegisterAccountRoutes(routes);
            RegisterPublicRoutes(routes);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeRavenDb();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }

        private void InitializeRavenDb()
        {
            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            RavenDbDocumentStore = new DocumentStore
            {
                ApiKey = parser.ConnectionStringOptions.ApiKey,
                Url = parser.ConnectionStringOptions.Url,
            };
            RavenDbDocumentStore.Initialize();
        }

        private static void RegisterAdminRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Admin",
                "admin",
                new { controller = "Admin", action = "Index" }
                );

            routes.MapRoute(
                "EpisodeAdmin",
                "admin/episodes/{action}/{episodeId}",
                new { controller = "EpisodeAdmin", episodeId = "" }
                );

            routes.MapRoute(
                "Settings",
                "admin/settings",
                new { controller = "SiteSettings", action = "Index" }
                );
        }

        private static void RegisterAccountRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "LogOn",
                "LogOn",
                new { controller = "Account", action = "LogOn" }
                );

            routes.MapRoute(
                "LogOff",
                "LogOff",
                new { controller = "Account", action = "LogOff" }
                );

            routes.MapRoute(
                "ChangePassword",
                "ChangePassword",
                new { controller = "Account", action = "ChangePassword" }
                );
        }

        private static void RegisterPublicRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Default",
                "",
                new { controller = "Home", action = "Index", id = "" }
                );

            routes.MapRoute(
                "Sitemap",
                "sitemap",
                new { controller = "Sitemap", action = "Sitemap" }
                );

            routes.MapRoute(
                "Feed",
                "rss/",
                new { controller = "Subscription", action = "RssRecent" }
                );

            routes.MapRoute(
                "FeedFull",
                "rss/full",
                new { controller = "Subscription", action = "RssFull" }
                );

            routes.MapRoute(
                "iTunes",
                "itunes/",
                new { controller = "Subscription", action = "Itunes" }
                );

            routes.MapRoute(
                "Zune",
                "zune/",
                new { controller = "Subscription", action = "Zune" }
                );

            routes.MapRoute(
                "Facebook",
                "facebook/",
                new { controller = "Subscription", action = "Facebook" }
                );

            routes.MapRoute(
                "Twitter",
                "twitter/",
                new { controller = "Subscription", action = "Twitter" }
                );

            routes.MapRoute(
                "Search",
                "search/{query}",
                new { controller = "Search", action = "Index", query = "" }
                );

            routes.MapRoute(
                "Archive",
                "archive",
                new { controller = "Archive", action = "Index" }
                );

            routes.MapRoute(
                 "Episode",
                 "{slug}",
                 new { controller = "Episode", action = "Permalink", slug = "" }
                 );
        }
    }
}
