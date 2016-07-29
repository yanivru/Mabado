using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using System.Linq.Expressions;

namespace Mabado.View
{
    public class ViewsContainer
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<Type, ViewConfiguration> _windowTypeToConfiguration;


        public ViewsContainer(IUnityContainer container)
        {
            _container = container;
            _windowTypeToConfiguration = new Dictionary<Type, ViewConfiguration>();
        }

        public ViewConfiguration With<TWindow>() where TWindow : Window
        {
            if (!_windowTypeToConfiguration.ContainsKey(typeof(TWindow)))
            {
                _windowTypeToConfiguration[typeof(TWindow)] = new ViewConfiguration();
            }

            return _windowTypeToConfiguration[typeof(TWindow)];
        }

        public TWindow ResolveWindow<TWindow>() where TWindow : Window
        {
            ViewConfiguration configuration = _windowTypeToConfiguration[typeof(TWindow)];

            TWindow window = _container.Resolve<TWindow>();

            var viewModel = CreateViewModel<TWindow>(configuration.ViewModelType, window);
            window.DataContext = viewModel;

            SetViewModelsCommandProperties<TWindow>(configuration.ViewModelType, configuration, window, viewModel);

            return window;
        }

        private void SetViewModelsCommandProperties<TWindow>(Type viewModelType, ViewConfiguration viewConfiguration, Window window, object viewModel) where TWindow : Window
        {
            PropertyInfo[] viewModelProperties = viewModelType.GetProperties().Where(x => x.PropertyType == typeof(ICommand)).ToArray();
            Dictionary<string, PropertyInfo> viewModelICommandProperties = GetCommandProperties(viewModelProperties).ToDictionary(p => p.Name);

            foreach (var viewModelProperty in viewModelProperties)
            {
                var commandFactory = viewConfiguration.Get_CommandFactory_ForViewModelField(viewModelProperty.Name);

                var command = commandFactory(window, viewModel);

                viewModelProperty.SetValue(viewModel, command);
            }
        }

        private static IEnumerable<PropertyInfo> GetCommandProperties(IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(p => typeof(ICommand).IsAssignableFrom(p.PropertyType));
        }

        private object CreateViewModel<TWindow>(Type viewModelType, Window window) where TWindow : Window
        {
            var viewModel = _container.Resolve(viewModelType,
                new DependencyOverride<Window>(window),
                new DependencyOverride<TWindow>(window));
            return viewModel;
        }
    }
}