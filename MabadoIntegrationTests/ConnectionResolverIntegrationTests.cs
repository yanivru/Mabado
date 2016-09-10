using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mabado.View.Factories;
using Microsoft.Practices.Unity;
using Mabado.View;
using Mabado.View.ViewModels;
using Mabado.View.Domain;
using Moq;
using System.IO;
using System.IO.Compression;
using System;
using InfrastructureStandAlone;
using System.Configuration;

namespace ConnectionResolverIntegrationTests
{
    [TestClass]
    public class ConnectionResolverTests
    {
        [TestMethod]
        public void UpdateConfigsCommandIsExecuted_ItChecksOutAndUpdatesTheConfigFiles()
        {
            var filledConnectionInfo = new ConnectionInfo()
            {
                Host = "expectedHost",
                DataSource = "expectedDataSource",
                Instance = "expectedInstance",
                UserName = "expectedUserName",
                Password = "expectedPasseword"
            };

            var installationInfo = new InstallationInfo()
            {
                UserSid = 123,
                Version = "3.4.5"
            };

            DeleteBranchFolder();
            DeployBranchFolder();
            ChangeCurrentDirectoryToDaSourceFolder();

            ConnectionResolverViewModel viewModel = CreateViewModel();

            viewModel.InputConnectionDetails = new ConnectionInfoViewModel(filledConnectionInfo);
            viewModel.SelectedLabInfoViewModel = new LabInfoViewModel(new LabInfo(filledConnectionInfo, installationInfo));

            viewModel.UpdateConfigsCommand.Execute(null);


            Configuration daConfiguration = OpenConfig(@"Windows\DataVantage.Windows\app.config");
            Configuration mcConfiguration = OpenConfig(@"..\..\ManagementConsole\Source\Varonis.UI.ManagementConsole.Shell\app.config");
            Configuration serverConfiguration = OpenConfig(@"..\..\Server\Source\Varonis.Server.WindowsService\app.config");

            Assert.AreEqual("3.4.5", daConfiguration.AppSettings.Settings["ProductVersion"].Value);
            Assert.AreEqual("123", daConfiguration.AppSettings.Settings["IMPERSONATION_SIDID"].Value);
            Assert.AreEqual("3.4.5", mcConfiguration.AppSettings.Settings["ProductVersion"].Value);
            Assert.AreEqual("123", mcConfiguration.AppSettings.Settings["IMPERSONATION_SIDID"].Value);
            Assert.AreEqual(@"Data Source=expectedDataSource\expectedInstance;Integrated Security=False;User ID=expectedUserName;Password=expectedPasseword",
                ((ConnectionStringsSection)serverConfiguration.GetSection("connectionStrings")).ConnectionStrings["vrnsDomainDB"].ConnectionString);

            RestoreCurrentFolder();
            DeleteBranchFolder();
        }

        private void RestoreCurrentFolder()
        {
            Directory.SetCurrentDirectory(@"..\..\..\");
        }

        private static void DeleteBranchFolder()
        {
            if (Directory.Exists("Branch"))
                Directory.Delete("Branch", true);
        }

        private static void DeployBranchFolder()
        {
            var zippedBranch = new MemoryStream(MabadoIntegrationTests.Properties.Resources.Branch);

            var zip = new ZipArchive(zippedBranch);
            zip.ExtractToDirectory(Directory.GetCurrentDirectory());
        }

        private static void ChangeCurrentDirectoryToDaSourceFolder()
        {
            Directory.SetCurrentDirectory(@".\Branch\Client\Source");
        }

        private static ConnectionResolverViewModel CreateViewModel()
        {
            var container = new UnityContainer();

            var viewsContainer = new ViewsContainer(container);

            MabadoViewBootLoader viewBootLoader = new MabadoViewBootLoader(container, viewsContainer);

            viewBootLoader.RegisterConnectionResolverModule();

            OverrideMockedDependencies(container);

            ConnectionResolverView view = viewsContainer.ResolveWindow<ConnectionResolverView>();

            var viewModel = view.DataContext as ConnectionResolverViewModel;
            return viewModel;
        }

        private static Configuration OpenConfig(string relativeFilePath)
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = relativeFilePath;
            return ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        private static void OverrideMockedDependencies(UnityContainer container)
        {
            var connectionStringVerifierMock = new Mock<IConnectionStringVerifier>();
            connectionStringVerifierMock.Setup(x => x.BeginVerify()).Callback(() => connectionStringVerifierMock.Object.ConnectionSucceeded(new ConnectionInfo()));
            container.RegisterInstance<Func<ConnectionInfo, IConnectionStringVerifier>>(connectionInfo => connectionStringVerifierMock.Object);

            var labInformationProviderMock = new Mock<ILabInformationProvider>();
            labInformationProviderMock.Setup(x => x.GetLabInfo(It.IsAny<String>())).Returns(new InstallationInfo() { UserSid = 43, Version = "1.2.3"});
            container.RegisterInstance<Func<ILabInformationProvider>>(() => labInformationProviderMock.Object);

            var sourceControlMock = new Mock<ISourceControl>();
            container.RegisterInstance(sourceControlMock.Object);

            container.RegisterType<IPathProvider, BranchPathProvider>();
        }
    }
}
