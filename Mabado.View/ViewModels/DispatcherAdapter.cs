using System;
using System.Windows;

namespace Mabado.View.ViewModels
{
    public class DispatcherAdapter : IDispatcher
    {
        public void BeginInvoke(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
