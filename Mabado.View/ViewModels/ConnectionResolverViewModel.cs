using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Mabado.View.Annotations;
using Mabado.View.Domain;

namespace Mabado.View.ViewModels
{
    public class ConnectionResolverViewModel : INotifyPropertyChanged
    {
        private ICommand _resolveConnectionStringCommand;
        private ICommand _updateConfigsCommand;
        private ICommand _readConfigsCommand;
        private ObservableCollection<LabInfoViewModel> _labInfoViewModels;
        private LabInfoViewModel _selectedLabInfoViewModel;
        private LabInfoViewModel _fromConfigLabInfoViewModel;
        private Visibility _resolveFailedWarningVisibility;
        private Visibility _resultsListBoxVisibility;

        public ObservableCollection<LabInfoViewModel> LabInfoViewModels
        {
            get { return _labInfoViewModels; }
            set
            {
                if (Equals(value, _labInfoViewModels))
                {
                    return;
                }
                _labInfoViewModels = value;
                OnPropertyChanged();
            }
        }

        public LabInfoViewModel FromConfigLabInfoViewModel
        {
            get { return _fromConfigLabInfoViewModel; }
            set
            {
                if (Equals(value, _fromConfigLabInfoViewModel))
                {
                    return;
                }
                _fromConfigLabInfoViewModel = value;
                OnPropertyChanged();
            }
        }

        public LabInfoViewModel SelectedLabInfoViewModel
        {
            get { return _selectedLabInfoViewModel; }
            set
            {
                if (Equals(value, _selectedLabInfoViewModel))
                {
                    return;
                }
                _selectedLabInfoViewModel = value;
                OnPropertyChanged();
            }
        }

        public ConnectionInfoViewModel InputConnectionDetails { get; set; }

        public Visibility IsSearching
        {
            get { return ResolveConnectionStringCommand.CanExecute(null) ? Visibility.Hidden : Visibility.Visible; }
        }

        public ICommand ResolveConnectionStringCommand
        {
            get { return _resolveConnectionStringCommand; }
            set
            {
                if (Equals(value, _resolveConnectionStringCommand))
                {
                    return;
                }

                if (_resolveConnectionStringCommand != null)
                {
                    _resolveConnectionStringCommand.CanExecuteChanged -= ResolveConnectionStringCommandOnCanExecuteChanged;
                }

                _resolveConnectionStringCommand = value;

                _resolveConnectionStringCommand.CanExecuteChanged += ResolveConnectionStringCommandOnCanExecuteChanged;

                OnPropertyChanged();
            }
        }

        private void ResolveConnectionStringCommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged("IsSearching");
        }

        public ICommand UpdateConfigsCommand
        {
            get { return _updateConfigsCommand; }
            set
            {
                if (Equals(value, _updateConfigsCommand)) return;
                _updateConfigsCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReadConfigsCommand
        {
            get { return _readConfigsCommand; }
            set
            {
                if (Equals(value, _readConfigsCommand)) return;
                _readConfigsCommand = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldUpdateConfigurationFiles { get; set; }

        public Visibility ResolveFailedWarningVisibility
        {
            get { return _resolveFailedWarningVisibility; }
            set
            {
                if (value.Equals(_resolveFailedWarningVisibility))
                {
                    return;
                }
                _resolveFailedWarningVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ResultsListBoxVisibility
        {
            get { return _resultsListBoxVisibility; }
            set
            {
                if (value == _resultsListBoxVisibility)
                {
                    return;
                }
                _resultsListBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public ConnectionResolverViewModel()
        {
            ResolveFailedWarningVisibility = Visibility.Collapsed;
            ResultsListBoxVisibility = Visibility.Visible;

            FromConfigLabInfoViewModel = new LabInfoViewModel(new LabInfo(new ConnectionInfo(), new InstallationInfo()));
            LabInfoViewModels = new ObservableCollection<LabInfoViewModel>();
            InputConnectionDetails = new ConnectionInfoViewModel(new ConnectionInfo());
            InputConnectionDetails.PropertyChanged += (sender, args) => ExecuteResolveConnectionString();
        }

        private void ExecuteResolveConnectionString()
        {
            if (_resolveConnectionStringCommand.CanExecute(null))
            {
                _resolveConnectionStringCommand.Execute(null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ConnectionInfoViewModel : INotifyPropertyChanged
    {
        private readonly ConnectionInfo _connectionInfo;

        public ConnectionInfoViewModel(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public string Host
        {
            get { return _connectionInfo.Host; }
            set
            {
                if (value == _connectionInfo.Host)
                {
                    return;
                }
                _connectionInfo.Host = value;
                OnPropertyChanged();
            }
        }

        public string Instance
        {
            get { return _connectionInfo.Instance; }
            set
            {
                if (value == _connectionInfo.Instance)
                {
                    return;
                }
                _connectionInfo.Instance = value;
                OnPropertyChanged();
            }
        }

        public string User
        {
            get { return _connectionInfo.UserName; }
            set
            {
                if (value == _connectionInfo.UserName)
                {
                    return;
                }
                _connectionInfo.UserName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _connectionInfo.Password; }
            set
            {
                if (value == _connectionInfo.Password)
                {
                    return;
                }
                _connectionInfo.Password = value;
                OnPropertyChanged();
            }
        }

        public ConnectionInfo ConnectionInfo
        {
            get { return _connectionInfo; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class LabInfoViewModel
    {
        private readonly LabInfo _labInfo;

        public LabInfoViewModel(LabInfo labInfo)
        {
            _labInfo = labInfo;
        }

        public LabInfo LabInfo
        {
            get { return _labInfo; }
        }

        public string DataSource
        {
            get { return _labInfo.ConnectionInfo.DataSource; }
        }

        public string User
        {
            get { return _labInfo.ConnectionInfo.UserName; }
        }

        public string Password
        {
            get { return _labInfo.ConnectionInfo.Password; }
        }

        public int UserSidId
        {
            get { return _labInfo.InstallationInfo.UserSid; }
        }

        public string Version
        {
            get { return _labInfo.InstallationInfo.Version; }
        }

        public string FormattedDescription
        {
            get { return string.Format("{0}\n{1}\n{2}\n\n{3}\n{4}", DataSource, User, Password, Version, UserSidId); }
        }
    }

    public class LabInfo
    {
        public LabInfo(ConnectionInfo connectionInfo, InstallationInfo installationInfo)
        {
            ConnectionInfo = connectionInfo;
            InstallationInfo = installationInfo;
        }

        public ConnectionInfo ConnectionInfo { get; private set; }

        public InstallationInfo InstallationInfo { get; private set; }
    }
}
