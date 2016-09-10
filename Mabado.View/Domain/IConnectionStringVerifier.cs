using System;

namespace Mabado.View.Domain
{
    public interface IConnectionStringVerifier
    {
        Action<ConnectionInfo, Exception> ConnectionFailed { get; set; }
        Action<ConnectionInfo> ConnectionSucceeded { get; set; }

        void BeginVerify();
    }
}