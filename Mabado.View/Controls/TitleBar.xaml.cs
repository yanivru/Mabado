using System.Windows;
using System.Windows.Input;

namespace Mabado.View.Controls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.DragMove();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Close();
        }
    }
}
