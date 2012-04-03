using MiniHawraman.Core.Services.Implementations;
using MiniHawraman.Core.Services.Interfaces;
using Ninject.Modules;

namespace MiniHawraman.Core.BindingModules
{
    public class RepositoriesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEpisodeService>().To<EpisodeService>();
            Bind<IUserService>().To<UserService>();
            Bind<IConfigurationService>().To<ConfigurationService>();
            Bind<ISiteService>().To<SiteService>();
            Bind<IFormsAuthentication>().To<FormsAuthenticationService>();
            Bind<IMembershipService>().To<MembershipService>();
        }
    }
}
