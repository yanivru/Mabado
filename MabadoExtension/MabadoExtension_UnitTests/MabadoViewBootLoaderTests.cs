using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;
using Mabado.View;
using Mabado.View.ViewModels;
using Mabado.View.Domain;
using Moq;

namespace MabadoExtension_UnitTests
{
    [TestClass]
    public class MabadoViewBootLoaderTests
    {
        [TestMethod]
        public void ResolveConnectionStringCommandIsExecuted_ItIsCalledWithSameViewModelThatTheViewContains()
        {
            var container = new UnityContainer();

            var viewsContainer = new ViewsContainer(container);

            MabadoViewBootLoader viewBootLoader = new MabadoViewBootLoader(container, viewsContainer);

            viewBootLoader.RegisterConnectionResolverModule();

            Mock<IConnectionStringResolver> connectionStringResolverMock = OverrideMockedDependencies(container);

            ConnectionResolverView view = viewsContainer.ResolveWindow<ConnectionResolverView>();

            var viewModel = view.DataContext as ConnectionResolverViewModel;

            var filledConnectionInfo = new ConnectionInfo()
            {
                Host = "expectedHost",
                DataSource = "expectedDataSource",
                Instance = "expectedInstance",
                UserName = "expectedUserName",
                Password = "expectedPasseword"
            };

            viewModel.InputConnectionDetails = new ConnectionInfoViewModel(filledConnectionInfo);

            viewModel.ResolveConnectionStringCommand.Execute(null);

            connectionStringResolverMock.Verify(x => x.Resolve(filledConnectionInfo), Times.Once);
        }

        private static Mock<IConnectionStringResolver> OverrideMockedDependencies(UnityContainer container)
        {
            var connectionStringResolverMock = new Mock<IConnectionStringResolver>();
            container.RegisterInstance(connectionStringResolverMock.Object);

            var labDetailsProviderMock = new Mock<ILabDetailsProvider>();
            container.RegisterInstance(labDetailsProviderMock.Object);

            var sourceControlMock = new Mock<ISourceControl>();
            container.RegisterInstance(sourceControlMock.Object);

            var pathProviderMock = new Mock<IPathProvider>();
            container.RegisterInstance(pathProviderMock.Object);
            return connectionStringResolverMock;
        }
    }
}
