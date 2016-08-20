using System;

namespace Mabado.View.Domain
{
    public interface IConnectionStringResolver
    {
        Action<ConnectionInfo, Exception> ConnectionFailed { get; set; }
        Action<ConnectionInfo> ConnectionSucceeded { get; set; }
        Action ResolveCompleted { get; set; }

        void Resolve(ConnectionInfo info);
    }
}