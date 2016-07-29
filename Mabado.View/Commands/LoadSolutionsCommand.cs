using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Mabado.View.Domain;
using Mabado.View.ViewModels;

namespace Mabado.View.Commands
{
    class LoadSolutionsCommand : ICommand
    {
        private readonly SolutionsLauncherViewModel _solutionsLauncherViewModel;
        private readonly ISolutionPriorityResolver _solutionPriorityResolver;
        private readonly ISolutionDetector _solutionDetector;
        private IDispatcher _dispatcher;

        public LoadSolutionsCommand(SolutionsLauncherViewModel solutionsLauncherViewModel, ISolutionDetector solutionDetector, ISolutionPriorityResolver solutionPriorityResolver, IDispatcher dispatcher)
        {
            _solutionsLauncherViewModel = solutionsLauncherViewModel;
            _solutionDetector = solutionDetector;
            _solutionPriorityResolver = solutionPriorityResolver;
            _dispatcher = dispatcher;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _solutionsLauncherViewModel.LoadingVisibility = Visibility.Visible;

            var solutionViewModels = new List<SolutionViewModel>();
            _solutionsLauncherViewModel.SiblingsSolutions = CollectionViewSource.GetDefaultView(solutionViewModels);          
            _solutionsLauncherViewModel.SiblingsSolutions.SortDescriptions.Add(new SortDescription("Priority", ListSortDirection.Descending));
            _solutionsLauncherViewModel.SiblingsSolutions.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            _solutionDetector.SolutionDetected = solution => AddSolutionToViewModel(solutionViewModels, solution);

            _solutionDetector.DetectAsync();
        }

        private void AddSolutionToViewModel(ICollection<SolutionViewModel> solutions, string fullFilePath)
        {
            var action = new Action(() =>
                {
                    solutions.Add(new SolutionViewModel
                        {
                            FullFilePath = fullFilePath,
                            Priority = _solutionPriorityResolver.Resolve(fullFilePath)
                        });

                    _solutionsLauncherViewModel.SiblingsSolutions.Refresh();

                    _solutionsLauncherViewModel.LoadingVisibility = Visibility.Collapsed;
                });

            _dispatcher.BeginInvoke(action);
        }

        public event EventHandler CanExecuteChanged;
    }
}