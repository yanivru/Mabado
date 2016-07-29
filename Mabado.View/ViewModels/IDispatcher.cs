using System;

namespace Mabado.View.ViewModels
{
    public interface IDispatcher
    {
        void BeginInvoke(Action action);
    }
}