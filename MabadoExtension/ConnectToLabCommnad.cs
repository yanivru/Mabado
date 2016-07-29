using Mabado.View;

namespace Reopened.MabadoExtension
{
    internal class ConnectToLabCommnad
    {
        private readonly ConnectionResolverView _connectionResolverView;

        public ConnectToLabCommnad(ConnectionResolverView connectionResolverView)
        {
            _connectionResolverView = connectionResolverView;
        }

        public void Execute()
        {
            _connectionResolverView.ShowDialog();
        }
    }
}