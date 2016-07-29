using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    public class ConnectionStringVerifier
    {
        private readonly ConnectionInfo _connectionInfo;
        private Task _task;
        private SqlConnection _connection;

        public Action<ConnectionInfo, Exception> ConnectionFailed { get; set; }
        public Action<ConnectionInfo> ConnectionSucceeded { get; set; }

        public ConnectionStringVerifier(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public void BeginVerify()
        {
            string connectionString = ConnectionInfoToConnectionString(_connectionInfo);
            _connection = new SqlConnection(connectionString);
            _task = _connection.OpenAsync();
            _task.GetAwaiter().OnCompleted(OnOpenComplete);
        }

        private void OnOpenComplete()
        {
            _connection.Close();
            _connection.Dispose();

            if (_task.IsFaulted)
            {
                RaiseConnectionFailed();
            }
            else
            {
                RaiseConnectionSucceeded();
            }

            _task.Dispose(); 
        }

        private void RaiseConnectionSucceeded()
        {
            if (ConnectionSucceeded != null)
            {
                ConnectionSucceeded(_connectionInfo);
            }
        }

        private void RaiseConnectionFailed()
        {
            if (ConnectionFailed != null)
            {
                ConnectionFailed(_connectionInfo, _task.Exception);
            }
        }

        private static string ConnectionInfoToConnectionString(ConnectionInfo connectionInfo)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    ConnectTimeout = 1
                };

            connectionInfo.UpdateConnectionString(connectionStringBuilder);

            return connectionStringBuilder.ConnectionString;
        }
    }
}