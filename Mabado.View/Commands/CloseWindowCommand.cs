using System;
using System.Windows;
using System.Windows.Input;

namespace Mabado.View.Commands
{
    internal class CloseWindowCommand : ICommand
    {
        private readonly Window _window;

        public CloseWindowCommand(Window window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.Close();
        }

        public event EventHandler CanExecuteChanged;
    }
}