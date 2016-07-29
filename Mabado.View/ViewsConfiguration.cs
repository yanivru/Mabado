using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace Mabado.View
{
    public class ViewConfiguration
    {
        public delegate ICommand CommandFactory(Window view, object viewModel);

        Dictionary<string, CommandFactory> _commandFactory_PerFieldName = new Dictionary<string, CommandFactory>();
        Type _viewModelType;

        public Type ViewModelType { get { return _viewModelType; } }

        public ViewConfiguration RegisterCommand<TViewModel>(Expression<Func<TViewModel, ICommand>> commandExpression, CommandFactory commandFactory)
        {
            string memberName = GetMemberName(commandExpression);
            _commandFactory_PerFieldName.Add(memberName, commandFactory);
            return this;
        }

        public ViewConfiguration RegisterViewModel<TViewModel>()
        {
            _viewModelType = typeof(TViewModel);
            return this;
        }

        public CommandFactory Get_CommandFactory_ForViewModelField(string fieldName)
        {
            return _commandFactory_PerFieldName[fieldName];
        }

        private string GetMemberName<TViewModel>(Expression<Func<TViewModel, ICommand>> commandExpression)
        {
            var expression = (MemberExpression)commandExpression.Body;
            return expression.Member.Name;
        }
    }
}
