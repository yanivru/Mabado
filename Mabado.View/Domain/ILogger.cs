using System;

namespace Mabado.View.Domain
{
    public interface ILogger
    {
        void WriteError(Exception exception);
        void WriteInfo(string message);
    }
}
