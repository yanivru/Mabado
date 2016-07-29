using System;
using System.ComponentModel;
using System.Windows.Input;
using Mabado.View.ViewModels;

namespace Mabado.View.Commands
{
    public class UpdateConfigsCommand : ICommand
    {
        private readonly ConnectionResolverViewModel _connectionResolverViewModel;
        private readonly ConnectionResolverView _window;

        public UpdateConfigsCommand(ConnectionResolverViewModel connectionResolverViewModel, ConnectionResolverView window)
        {
            _connectionResolverViewModel = connectionResolverViewModel;
            _window = window;

            _connectionResolverViewModel.PropertyChanged += ConnectionResolverViewModelOnPropertyChanged;
        }

        private void ConnectionResolverViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _connectionResolverViewModel.SelectedLabInfoViewModel != null;
        }

        public void Execute(object parameter)
        {
            _window.DialogResult = true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
