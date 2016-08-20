using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Mabado.View.Domain;
using Mabado.View.ViewModels;

namespace Mabado.View.Commands
{
    public class ResolveConnectionStringCommand : ICommand
    {
        private readonly ConnectionResolverViewModel _connectionResolverViewModel;
        private readonly ILabDetailsProvider _labDetailsProvider;
        private readonly IConnectionInfoGuesser _connectionInfoGuesser;
        private bool _canExecute;
        private IConnectionStringResolver _connectionStringResolver;

        private bool InnerCanExecute
        {
            get { return _canExecute; }
            set
            {
                _canExecute = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public ResolveConnectionStringCommand(ConnectionResolverViewModel connectionResolverViewModel, IConnectionStringResolver connectionStringResolver, ILabDetailsProvider labDetailsProvider)
        {
            _connectionStringResolver = connectionStringResolver;
            _connectionResolverViewModel = connectionResolverViewModel;
            _labDetailsProvider = labDetailsProvider;
            InnerCanExecute = true;
        }

        public bool CanExecute(object parameter)
        {
            return InnerCanExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            InnerCanExecute = false;

            _connectionResolverViewModel.LabInfoViewModels.Clear();

            _connectionResolverViewModel.ResolveFailedWarningVisibility = Visibility.Collapsed;
            _connectionResolverViewModel.ResultsListBoxVisibility = Visibility.Visible;

            _connectionStringResolver.ConnectionSucceeded = ConnectionSucceeded;
            _connectionStringResolver.ResolveCompleted = ResolveCompleted;
            _connectionStringResolver.Resolve(_connectionResolverViewModel.InputConnectionDetails.ConnectionInfo);
        }

        private void ResolveCompleted()
        {
            _connectionResolverViewModel.ResolveFailedWarningVisibility = !_connectionResolverViewModel.LabInfoViewModels.Any() ? Visibility.Visible : Visibility.Collapsed;
            _connectionResolverViewModel.ResultsListBoxVisibility = _connectionResolverViewModel.LabInfoViewModels.Any() ? Visibility.Visible : Visibility.Collapsed;
            InnerCanExecute = true;
        }

        private void ConnectionSucceeded(ConnectionInfo connectionInfo)
        {
            InstallationInfo installationInfo = _labDetailsProvider.GetDetails(connectionInfo);

            LabInfo labInfo = new LabInfo(connectionInfo, installationInfo);

            _connectionResolverViewModel.LabInfoViewModels.Add(new LabInfoViewModel(labInfo));
            _connectionResolverViewModel.SelectedLabInfoViewModel = _connectionResolverViewModel.LabInfoViewModels[0];
        }
    }
}