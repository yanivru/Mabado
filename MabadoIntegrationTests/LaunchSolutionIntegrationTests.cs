using Mabado.View.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Windows;
using System.Linq;
using Mabado.View;
using Mabado.View.Factories;
using Moq;
using Mabado.View.Domain;
using InfrastructureStandAlone;
using Microsoft.Practices.Unity;
using System.IO;

namespace MabadoExtension_UnitTests
{
    [TestClass]
    public class LaunchSolutionIntegrationTests
    {
        private const string solutionFileName = @"C:\Temp\MabadoTests\SomeFolder\SolutionFolder\a.sln";
        private const string sibllingSolutionFileName = @"C:\Temp\MabadoTests\SomeFolder\b.sln";

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory(@"C:\Temp\MabadoTests\SomeFolder\SolutionFolder");
            File.WriteAllText(solutionFileName, "Mock solution file");
            File.WriteAllText(sibllingSolutionFileName, "Mock solution file");
            Directory.SetCurrentDirectory(@"C:\Temp\MabadoTests\SomeFolder\SolutionFolder");
        }

        [TestMethod]
        public void LoadSolutionsFromFolder_LaunchOneOfTheSolutions()
        {
            Window view = LaunchSolutionViewFactory.GetLaunchSolutionView();

            SolutionsLauncherViewModel viewModel = (SolutionsLauncherViewModel)view.DataContext;

            viewModel.LoadSolutions.Execute(null);

            WaitFor(() => viewModel.SiblingsSolutions.OfType<SolutionViewModel>().Count() == 2);

            VerifyAllExpectedSolutionsWhereLoaded(viewModel);

            LaunchSolutionAndVerifySolutionLauncherWasCalled(viewModel);

        }

        private void LaunchSolutionAndVerifySolutionLauncherWasCalled(SolutionsLauncherViewModel viewModel)
        {
            viewModel.SiblingsSolutions.MoveCurrentToFirst();
            viewModel.SelectedSolution = (SolutionViewModel)viewModel.SiblingsSolutions.CurrentItem;
            viewModel.LaunchSolutionCommand.Execute(null);

            LaunchSolutionViewFactory.SolutionLauncherMock.Verify(x => x.OpenSolution(solutionFileName));
        }

        private static void VerifyAllExpectedSolutionsWhereLoaded(SolutionsLauncherViewModel viewModel)
        {
            SolutionViewModel[] expected = new[] {
                new SolutionViewModel
                        {
                            FullFilePath = solutionFileName,
                            Priority = 0
                        },
            new SolutionViewModel
                        {
                            FullFilePath = sibllingSolutionFileName,
                            Priority = 0
                        }};

            var expectedArray = expected.ToArray();
            var actualArray = viewModel.SiblingsSolutions.Cast<SolutionViewModel>().ToArray();

            CollectionAssert.AreEquivalent(expectedArray, actualArray);
        }

        private void WaitFor(Func<bool> predicate)
        {
            DateTime start = DateTime.Now;
            while (!predicate())
            {
                Thread.Sleep(100);

                if ((DateTime.Now - start).TotalSeconds > 50)
                {
                    throw new TimeoutException();
                }
            }
        }
    }

    public class LaunchSolutionViewFactory
    {
        public static Mock<ISolutionLauncher> SolutionLauncherMock;

        public static LaunchSolutionView GetLaunchSolutionView()
        {
            UnityContainer container = CreateIocContainer();

            var bootLoader = container.Resolve<MabadoViewBootLoader>();
            bootLoader.RegisterSolutionLauncherModule();

            RegisterMockDispatcher(container);
            RegisterMockSolutionLauncher(container);

            ViewsContainer viewsContainer = container.Resolve<ViewsContainer>();

            return viewsContainer.ResolveWindow<LaunchSolutionView>();
        }

        private static void RegisterMockSolutionLauncher(UnityContainer container)
        {
            SolutionLauncherMock = new Mock<ISolutionLauncher>();

            container.RegisterInstance<ISolutionLauncher>(SolutionLauncherMock.Object);
        }

        private static void RegisterMockDispatcher(UnityContainer container)
        {
            Mock<IDispatcher> dispatcherMock = new Mock<IDispatcher>();
            dispatcherMock.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback<Action>((a) => a());
            container.RegisterInstance(dispatcherMock.Object);
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
