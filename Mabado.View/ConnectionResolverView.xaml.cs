using System;
using System.Windows;
using Mabado.View.ViewModels;

namespace Mabado.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConnectionResolverView
    {
        public ConnectionResolverView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var connectionResolverViewModel = DataContext as ConnectionResolverViewModel;

            if (connectionResolverViewModel == null)
            {
                return;
            }

            if (connectionResolverViewModel.ReadConfigsCommand.CanExecute(null))
            {
                connectionResolverViewModel.ReadConfigsCommand.Execute(null);
            }
        }
    }
}
