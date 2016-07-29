using System;
using System.Collections.Generic;
using System.Windows.Input;
using Mabado.View.Domain;
using Mabado.View.ViewModels;

namespace Mabado.View.Commands
{
    public class UpdateConfigFilesCommand : ICommand
    {
        private readonly ConnectionResolverViewModel _connectionResolverViewModel;
        private readonly ISolutionConnectionStrings _serverSolutionConnectionStrings;
        private readonly IEnumerable<ILabInfoProvider> _labInfoProviders;


        public UpdateConfigFilesCommand(ConnectionResolverViewModel connectionResolverViewModel, ISolutionConnectionStrings serverSolutionConnectionStrings, IEnumerable<ILabInfoProvider> labInfoProviders)
        {
            _connectionResolverViewModel = connectionResolverViewModel;
            _serverSolutionConnectionStrings = serverSolutionConnectionStrings;
            _labInfoProviders = labInfoProviders;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _serverSolutionConnectionStrings.Update(_connectionResolverViewModel.SelectedLabInfoViewModel.LabInfo.ConnectionInfo);
            
            foreach (var labInfoProvider in _labInfoProviders)
            {
                labInfoProvider.Update(_connectionResolverViewModel.SelectedLabInfoViewModel.LabInfo.InstallationInfo);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}