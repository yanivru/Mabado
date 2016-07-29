using System;
using System.Windows.Input;
using Mabado.View.ViewModels;
using Mabado.View.Domain;

namespace Mabado.View.Commands
{
    internal class LaunchSolutionCommand : ICommand
    {
        private readonly SolutionsLauncherViewModel _solutionsLauncherViewModel;
        private readonly bool _terminateWhenDone;
        private readonly ISolutionLauncher _solutionLauncher;

        public LaunchSolutionCommand(SolutionsLauncherViewModel solutionsLauncherViewModel, ISolutionLauncher solutionLauncher, bool terminateWhenDone = false)
        {
            _solutionsLauncherViewModel = solutionsLauncherViewModel;
            _terminateWhenDone = terminateWhenDone;
            _solutionLauncher = solutionLauncher;

            _solutionsLauncherViewModel.PropertyChanged += (sender, args) => OnCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return _solutionsLauncherViewModel.SelectedSolution != null;
        }

        public void Execute(object parameter)
        {
            SolutionViewModel solutionViewModel = _solutionsLauncherViewModel.SelectedSolution;

            _solutionLauncher.OpenSolution(solutionViewModel.FullFilePath);

            if (_terminateWhenDone)
            {
                _solutionsLauncherViewModel.Terminate();
            }
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}