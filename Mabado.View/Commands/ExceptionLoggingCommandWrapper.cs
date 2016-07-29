using System;
using System.Windows;
using System.Windows.Input;
using Mabado.View.Domain;

namespace Mabado.View.Commands
{
    class ExceptionLoggingCommandWrapper : ICommand
    {
        private readonly ICommand _innerCommand;
        private readonly ILogger _logger;

        public ExceptionLoggingCommandWrapper(ICommand innerCommand, ILogger logger)
        {
            _innerCommand = innerCommand;
            _logger = logger;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _innerCommand.CanExecute(parameter);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return false;
            }
        }

        public void Execute(object parameter)
        {
            try
            {
                _innerCommand.Execute(parameter);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                MessageBox.Show("An error occured.\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { _innerCommand.CanExecuteChanged += value; }
            remove { _innerCommand.CanExecuteChanged -= value; }
        }
    }
}
