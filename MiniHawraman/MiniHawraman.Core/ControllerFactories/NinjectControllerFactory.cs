using System;
using System.Web.Mvc;
using MiniHawraman.Core.BindingModules;
using Ninject;

namespace MiniHawraman.Core.ControllerFactories
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel = new StandardKernel(new RepositoriesModule());

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;
            return (IController)kernel.Get(controllerType);
        }
    }
}
