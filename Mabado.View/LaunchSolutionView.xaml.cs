using System.Windows.Input;
using Mabado.View.ViewModels;
using System.Windows;

namespace Mabado.View
{
    /// <summary>
    /// Interaction logic for LaunchSolutionView.xaml
    /// </summary>
    public partial class LaunchSolutionView
    {
        public LaunchSolutionView()
        {
            InitializeComponent();

            Loaded += LaunchSolutionView_Loaded;
            Listbox1.MouseDoubleClick += listBox1_MouseDoubleClick;
        }

        void LaunchSolutionView_Loaded(object sender, RoutedEventArgs e)
        {
            SolutionsLauncherViewModel viewModel = (SolutionsLauncherViewModel)DataContext;

            viewModel.LoadSolutions.Execute(null);
        }

        void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SolutionsLauncherViewModel viewModel = (SolutionsLauncherViewModel)DataContext;

            if (viewModel.LaunchSolutionAndCloseExit.CanExecute(null))
            {
                viewModel.LaunchSolutionAndCloseExit.Execute(null);
            }
        }
    }
}
