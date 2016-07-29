using System;
using System.Windows.Input;

namespace Mabado.View.Commands
{
    internal class CompositeCommand : ICommand
    {
        private readonly ICommand _secondCommand;
        private readonly ICommand _firstCommand;

        public CompositeCommand(ICommand firstCommand, ICommand secondCommand)
        {
            _secondCommand = secondCommand;
            _firstCommand = firstCommand;
        }

        public bool CanExecute(object parameter)
        {
            return _secondCommand.CanExecute(parameter) && _firstCommand.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _firstCommand.Execute(parameter);
            _secondCommand.Execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _secondCommand.CanExecuteChanged += value;
                _firstCommand.CanExecuteChanged += value;
            }
            remove
            {
                _secondCommand.CanExecuteChanged -= value;
                _firstCommand.CanExecuteChanged -= value;
            }
        }
    }
}