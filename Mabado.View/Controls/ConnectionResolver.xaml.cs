using System.Windows;
using Mabado.View.ViewModels;

namespace Mabado.View.Controls
{
    /// <summary>
    /// Interaction logic for ConnectionResolver.xaml
    /// </summary>
    public partial class ConnectionResolver
    {
        public ConnectionResolver()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((ConnectionResolverViewModel)DataContext).ReadConfigsCommand.Execute(null);
        }
    }
}
