using EnvDTE80;
using Infrastructre;
using Mabado.View;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;

namespace Reopened.MabadoExtension
{
    class ConnectToLabFactory
    {
        public static ConnectToLabCommand CreateConnectToLabCommand(DTE2 dte)
        {
            UnityContainer container = RegisterConnectionResolverModule(dte);

//            ViewsContainer viewsContainer = container.Resolve<ViewsContainer>();

//            return viewsContainer.ResolveWindow<ConnectionResolverView>();

            return container.Resolve<ConnectToLabCommand>();
        }

        private static UnityContainer RegisterConnectionResolverModule(DTE2 dte)
        {
            UnityContainer container = CreateIocContainer(dte);

            var bootLoader = container.Resolve<MabadoViewBootLoader>();
            bootLoader.RegisterConnectionResolverModule();
            return container;
        }

        private static UnityContainer CreateIocContainer(DTE2 dte)
        {
            var logger = new OutputWindowLogger(dte);
            logger.WriteInfo("Loading Mabado... Connect to lab");

            UnityContainer container = new UnityContainer();
            container.RegisterInstance<ILogger>(logger);
            container.RegisterInstance<IPathProvider>(new BranchPathProvider(dte));
            container.RegisterInstance<ISourceControl>(new SourceControlAdapter(dte.SourceControl));
            container.RegisterInstance<IPathProvider>(new BranchPathProvider(dte));
            container.RegisterType<ViewsContainer, ViewsContainer>(new ContainerControlledLifetimeManager());
            return container;
        }

    }
}
