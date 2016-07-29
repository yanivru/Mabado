using InfrastructureStandAlone;
using Mabado.View;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;
using Moq;

namespace Emulator
{
    public class LaunchSolutionViewFactory
    {
        public static LaunchSolutionView GetLaunchSolutionView()
        {
            UnityContainer container = CreateIocContainer();

            var bootLoader = container.Resolve<MabadoViewBootLoader>();
            bootLoader.RegisterSolutionLauncherModule();

            ViewsContainer viewsContainer = container.Resolve<ViewsContainer>();

            return viewsContainer.ResolveWindow<LaunchSolutionView>();
        }

        private static UnityContainer CreateIocContainer()
        {
            Mock<ISourceControl> sourceControlMock = new Mock<ISourceControl>();
            sourceControlMock.Setup(control => control.CheckOutItem(It.IsAny<string>())).Returns(true);

            UnityContainer container = new UnityContainer();
            container.RegisterType<ILogger, MessageBoxLogger>();
            container.RegisterInstance(sourceControlMock.Object);
            container.RegisterType<IPathProvider, BranchPathProvider>();
            container.RegisterType<ViewsContainer, ViewsContainer>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}
