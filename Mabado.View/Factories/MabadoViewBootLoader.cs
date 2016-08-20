using System.Collections.Generic;
using Mabado.View.Domain;
using Mabado.View.Infrastructure;
using Microsoft.Practices.Unity;
using Mabado.View.ViewModels;
using Mabado.View.Commands;

namespace Mabado.View.Factories
{
    public class MabadoViewBootLoader
    {
        private readonly IUnityContainer _container;
        private ViewsContainer _viewsContainer;

        public MabadoViewBootLoader(IUnityContainer container, ViewsContainer viewsContainer)
        {
            _container = container;
            _viewsContainer = viewsContainer;
        }

        public void RegisterConnectionResolverModule()
        {
            RegisterConnectionResolverViewModel();
            RegisterLabInformationConfigProviders();
        }

        public void RegisterSolutionLauncherModule()
        {
            _container.RegisterType<ISolutionLauncher, SolutionLauncher>();

            _container.RegisterType<IDispatcher, DispatcherAdapter>();

            _container.RegisterType<ISolutionDetector, SolutionDetector>()
                .RegisterType<ISolutionPriorityResolver, HardcodedSolutionPriorityResolver>();

            _viewsContainer.With<LaunchSolutionView>()
                .RegisterViewModel<SolutionsLauncherViewModel>()
                .RegisterCommand<SolutionsLauncherViewModel>((vm) => vm.LoadSolutions, (view, viewModel) => _container.Resolve<LoadSolutionsCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<SolutionsLauncherViewModel>(viewModel)))
                .RegisterCommand<SolutionsLauncherViewModel>((vm) => vm.LaunchSolutionCommand, (view, viewModel) => _container.Resolve<LaunchSolutionCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<SolutionsLauncherViewModel>(viewModel), new DependencyOverride<bool>(false)))
                .RegisterCommand<SolutionsLauncherViewModel>((vm) => vm.LaunchSolutionAndCloseExit, (view, viewModel) => _container.Resolve<LaunchSolutionCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<SolutionsLauncherViewModel>(viewModel), new DependencyOverride<bool>(true)));
        }

        private void RegisterConnectionResolverViewModel()
        {
            _container.RegisterType<IDispatcher, DispatcherAdapter>();

            _container.RegisterType<ILabDetailsProvider, KeyValueLabDetailsProvider>(new InjectionConstructor("vrnsDomainDB"));
            _container.RegisterType<IConnectionInfoGuesser, ConnectionStringGueser>();
            _container.RegisterType<IConnectionStringResolver, ConnectionStringResolver>();

            _container.RegisterType<ResolveConnectionStringCommand, ResolveConnectionStringCommand>();

            _viewsContainer.With<ConnectionResolverView>()
                .RegisterViewModel<ConnectionResolverViewModel>()
                .RegisterCommand<ConnectionResolverViewModel>((vm) => vm.ResolveConnectionStringCommand, (view, viewModel) => _container.Resolve<ResolveConnectionStringCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<ConnectionResolverViewModel>(viewModel)))
                .RegisterCommand<ConnectionResolverViewModel>((vm) => vm.UpdateConfigsCommand, (view, viewModel) => _container.Resolve<UpdateConfigFilesCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<SolutionsLauncherViewModel>(viewModel)))
                .RegisterCommand<ConnectionResolverViewModel>((vm) => vm.ReadConfigsCommand, (view, viewModel) => _container.Resolve<ReadConfigsCommand>(new DependencyOverride<LaunchSolutionView>(view), new DependencyOverride<SolutionsLauncherViewModel>(viewModel)));
        }

        private void RegisterLabInformationConfigProviders()
        {
            _container.RegisterType<IEnumerable<ILabInfoProvider>, ILabInfoProvider[]>();
            _container.RegisterType<ILabInfoProvider, LabInfoConfigFileProvider>();
            _container.RegisterType<ISolutionConnectionStrings, SolutionConnectionStrings>();
            _container.RegisterType<IConfigurationProvider, ConfigurationProviderCheckOutWrapper>(
                new InjectionConstructor(
                    typeof(ConfigurationProvider),
                    typeof(ServerConnectionStringsConfigurationFilePathProvider),
                    typeof(ISourceControl)));
            _container.RegisterType<ConfigurationProvider>(
                new InjectionConstructor(typeof(ServerConfigurationFilePathProvider)));
            _container.RegisterType<ServerConfigurationFilePathProvider>(new InjectionConstructor(new ResolvedParameter(typeof(IPathProvider))));
            _container.RegisterType<ServerConnectionStringsConfigurationFilePathProvider>(new InjectionConstructor(new ResolvedParameter(typeof(IPathProvider))));

            _container.RegisterType<ILabInfoProvider, LabInfoConfigFileProvider>("Da",
                new InjectionConstructor(new ResolvedParameter(typeof(ConfigurationProviderCheckOutWrapper), "Da")));
            _container.RegisterType<ConfigurationProviderCheckOutWrapper>("Da",
                new InjectionConstructor(
                    new ResolvedParameter(typeof(ConfigurationProvider), "Da"),
                    typeof(DaConfigurationFilePathProvider),
                    typeof(ISourceControl)));
            _container.RegisterType<ConfigurationProvider>("Da", new InjectionConstructor(typeof(DaConfigurationFilePathProvider)));

            _container.RegisterType<ILabInfoProvider, LabInfoConfigFileProvider>("Mc",
                new InjectionConstructor(new ResolvedParameter(typeof(ConfigurationProviderCheckOutWrapper), "Mc")));
            _container.RegisterType<ConfigurationProviderCheckOutWrapper>("Mc",
                new InjectionConstructor(
                    new ResolvedParameter(typeof(ConfigurationProvider), "Mc"),
                    typeof(McConfigurationFilePathProvider),
                    typeof(ISourceControl)));
            _container.RegisterType<ConfigurationProvider>("Mc", new InjectionConstructor(typeof(McConfigurationFilePathProvider)));
        }
    }
}
