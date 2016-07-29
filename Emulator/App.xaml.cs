using System.Windows;
using Mabado.View;
using Mabado.View.Domain;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;
using Moq;
using System.IO;
using InfrastructureStandAlone;

namespace Emulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);

            var connectionResolverView = CreateConnectionResolverView();
            connectionResolverView.ShowDialog();

            //var launchSolutionView = LaunchSolutionViewFactory.GetLaunchSolutionView();
            //launchSolutionView.ShowDialog();
        }

        private static ConnectionResolverView CreateConnectionResolverView()
        {
            UnityContainer container = RegisterConnectionResolverModule();

            ViewsContainer viewsContainer = container.Resolve<ViewsContainer>();

            return viewsContainer.ResolveWindow<ConnectionResolverView>();
        }

        private static UnityContainer RegisterConnectionResolverModule()
        {
            UnityContainer container = CreateIocContainer();

            var bootLoader = container.Resolve<MabadoViewBootLoader>();
            bootLoader.RegisterConnectionResolverModule();
            return container;
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
