using System;
using System.Windows.Input;
using Mabado.View;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MabadoExtension_UnitTests
{
    [TestClass]
    public class ViewsContainerTests
    {
        [TestMethod]
        public void ResolveWindow_RegisterCommandWIthCommandFactory_CommandsAreResolvedByFactory()
        {
            UnityContainer container = new UnityContainer();

            var viewsContainer = new ViewsContainer(container);
            viewsContainer.With<System.Windows.Window>()
                .RegisterViewModel<DummyViewModel>()
                .RegisterCommand<DummyViewModel>((vm) => vm.CommandA, (v, vm) => new CommandB())
                .RegisterCommand<DummyViewModel>((vm) => vm.CommandB, (v, vm) => new CommandB());

            var resolvedWindow = viewsContainer.ResolveWindow<System.Windows.Window>();

            var dummyViewModel = (DummyViewModel)resolvedWindow.DataContext;
            Assert.IsInstanceOfType(dummyViewModel.CommandA, typeof(CommandB));
            Assert.IsInstanceOfType(dummyViewModel.CommandB, typeof(CommandB));
        }
    }

    public class DummyViewModel
    {
        public ICommand CommandA { get; set; }
        public ICommand CommandB { get; set; }
    }

    public class ViewModelWithOrphanCommand
    {
        public ICommand CommandA { get; set; }
        public ICommand CommandB { get; set; }
        public ICommand CommandC { get; set; }
    }

    public class CommandA : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class CommandB : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class CommandC : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }

}
