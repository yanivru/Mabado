using System;
using System.Windows.Input;
using Mabado.View;
using Mabado.View.Commands;
using Mabado.View.Domain;
using Mabado.View.ViewModels;
using Microsoft.Practices.Unity;

namespace Reopened.MabadoExtension
{
    public class ConnectToLabCommand : ICommand
    {
        private readonly IUnityContainer _container;
        private ViewsContainer _viewsContainer;

        public ConnectToLabCommand(IUnityContainer container, ViewsContainer viewsContainer)
        {
            _container = container;
            _viewsContainer = viewsContainer;
        }

        public void Execute(object parameter)
        {
            var connectionResolverView = _viewsContainer.ResolveWindow<ConnectionResolverView>();

            if (connectionResolverView.ShowDialog() == true)
            {
                // Workaround: We need to run this command only after the window was closed. If VS checkout dialog is launched from WPF screen VS thinks there is an open dialog even after everything was closed. (When closing VS a message will appear).
                var command = new UpdateConfigFilesCommand((ConnectionResolverViewModel)connectionResolverView.DataContext, _container.Resolve<ISolutionConnectionStrings>(), _container.ResolveAll<ILabInfoProvider>());
                command.Execute(null);
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}