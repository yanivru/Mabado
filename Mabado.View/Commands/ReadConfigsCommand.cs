using System;
using System.Windows.Input;
using Mabado.View.Domain;
using Mabado.View.ViewModels;

namespace Mabado.View.Commands
{
    public class ReadConfigsCommand : ICommand
    {
        private readonly ConnectionResolverViewModel _connectionResolverViewModel;
        private readonly ISolutionConnectionStrings _serverSolutionConnectionStrings;
        private readonly ILabInfoProvider _clientConfigFile;

        public ReadConfigsCommand(ConnectionResolverViewModel connectionResolverViewModel, ISolutionConnectionStrings serverSolutionConnectionStrings, ILabInfoProvider clientConfigFile)
        {
            _connectionResolverViewModel = connectionResolverViewModel;
            _serverSolutionConnectionStrings = serverSolutionConnectionStrings;
            _clientConfigFile = clientConfigFile;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ConnectionInfo connectionInfo = _serverSolutionConnectionStrings.Read();
            InstallationInfo installationInfo = _clientConfigFile.Read();

            LabInfo labInfo = new LabInfo(connectionInfo, installationInfo);

            _connectionResolverViewModel.FromConfigLabInfoViewModel = new LabInfoViewModel(labInfo);
        }

        public event EventHandler CanExecuteChanged;
    }
}