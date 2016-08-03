using EnvDTE80;
using Infrastructre;
using Mabado.View;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;

namespace Reopened.MabadoExtension
{
    public class LaunchSolutionViewFactory
    {
        public static LaunchSolutionView GetLaunchSolutionView(DTE2 dte)
        {
            UnityContainer container = CreateIocContainer(dte);

            var bootLoader = container.Resolve<MabadoViewBootLoader>();
            bootLoader.RegisterSolutionLauncherModule();

            ViewsContainer viewsContainer = container.Resolve<ViewsContainer>();

            return viewsContainer.ResolveWindow<LaunchSolutionView>();
        }

        private static UnityContainer CreateIocContainer(DTE2 dte)
        {
            var logger = new OutputWindowLogger(dte);
            logger.WriteInfo("Loading Mabado... Connect to lab");

            var branchProvider = new BranchPathProvider(dte);

            UnityContainer container = new UnityContainer();
            container.RegisterInstance<ILogger>(logger);
            container.RegisterInstance(new SourceControlAdapter(dte.SourceControl));
            container.RegisterInstance<IPathProvider>(branchProvider);
            container.RegisterType<ViewsContainer, ViewsContainer>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}
