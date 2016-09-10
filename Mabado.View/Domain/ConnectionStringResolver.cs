using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mabado.View.Infrastructure;

namespace Mabado.View.Domain
{
    public class ConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IConnectionInfoGuesser _connectionInfoGuesser;

        private int _progressCountDown;
        private Func<ConnectionInfo, IConnectionStringVerifier> _connectionStringVerifierFactory;

        public Action<ConnectionInfo, Exception> ConnectionFailed { get; set; }
        public Action<ConnectionInfo> ConnectionSucceeded { get; set; }
        public Action ResolveCompleted { get; set; }

        public ConnectionStringResolver(IConnectionInfoGuesser connectionInfoGuesser, Func<ConnectionInfo, IConnectionStringVerifier> connectionStringVerifierFactory)
        {
            _connectionInfoGuesser = connectionInfoGuesser;
            _connectionStringVerifierFactory = connectionStringVerifierFactory;
        }

        public void Resolve(ConnectionInfo info)
        {
            if (string.IsNullOrEmpty(info.Host))
            {
                throw new Exception("Cannot resolve connection string. IP cannot be null");
            }

            IEnumerable<ConnectionInfo> guessedConnectionInfos = _connectionInfoGuesser.Guess(info);

            IEnumerable<IConnectionStringVerifier> verifiers = guessedConnectionInfos.Select(connectionInfo =>
            {
                var verifier = _connectionStringVerifierFactory(connectionInfo);
                verifier.ConnectionFailed += ConnectionFailedHandler;
                verifier.ConnectionSucceeded += ConnectionSucceededHandler;
                return verifier;
            }).ToArray();

            _progressCountDown = verifiers.Count();

            foreach (ConnectionStringVerifier verifier in verifiers)
            {
                verifier.BeginVerify();
            }
        }

        private void ConnectionSucceededHandler(ConnectionInfo connectionInfo)
        {
            if (ConnectionSucceeded != null)
            {
                ConnectionSucceeded(connectionInfo);
            }

            CountDown();
        }

        private void ConnectionFailedHandler(ConnectionInfo connectionInfo, Exception exception)
        {
            if (ConnectionFailed != null)
            {
                ConnectionFailed(connectionInfo, exception);
            }

            CountDown();
        }

        private void CountDown()
        {
            if (Interlocked.Decrement(ref _progressCountDown) == 0)
            {
                if (ResolveCompleted != null)
                {
                    ResolveCompleted();
                }
            }
        }
    }
}