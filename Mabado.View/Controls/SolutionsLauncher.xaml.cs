using System.Windows;
using Mabado.View.ViewModels;

namespace Mabado.View.Controls
{
    /// <summary>
    /// Interaction logic for SolutionsLauncher.xaml
    /// </summary>
    public partial class SolutionsLauncher
    {
        public SolutionsLauncher()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((SolutionsLauncherViewModel)DataContext).LoadSolutions.Execute(null);
        }
    }
}
