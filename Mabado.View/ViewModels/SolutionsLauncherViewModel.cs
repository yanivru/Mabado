using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Mabado.View.Annotations;

namespace Mabado.View.ViewModels
{
    public class SolutionsLauncherViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private ICollectionView _siblingsSolutions;
        private ICommand _loadSolutions;
        private ICommand _launchSolutionCommand;
        private SolutionViewModel _selectedSolution;
        private ICommand _launchSolutionAndCloseExit;
        private Visibility _loadingVisibility;

        public SolutionsLauncherViewModel(Window window)
        {
            _window = window;
            LoadingVisibility = Visibility.Collapsed;
        }

        public ICollectionView SiblingsSolutions
        {
            get { return _siblingsSolutions; }
            set
            {
                if (Equals(value, _siblingsSolutions)) return;
                _siblingsSolutions = value;
                OnPropertyChanged();
            }
        }

        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set
            {
                if (value == _loadingVisibility)
                {
                    return;
                }
                _loadingVisibility = value;
                OnPropertyChanged();
            }
        }

        public SolutionViewModel SelectedSolution
        {
            get { return _selectedSolution; }
            set
            {
                if (Equals(value, _selectedSolution)) return;
                _selectedSolution = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadSolutions
        {
            get { return _loadSolutions; }
            set
            {
                if (Equals(value, _loadSolutions)) return;
                _loadSolutions = value;
                OnPropertyChanged();
            }
        }

        public ICommand LaunchSolutionCommand
        {
            get { return _launchSolutionCommand; }
            set
            {
                if (Equals(value, _launchSolutionCommand)) return;
                _launchSolutionCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand LaunchSolutionAndCloseExit
        {
            get { return _launchSolutionAndCloseExit; }
            set
            {
                if (Equals(value, _launchSolutionAndCloseExit))
                {
                    return;
                }
                _launchSolutionAndCloseExit = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Terminate()
        {
            _window.Close();
        }
    }
}
