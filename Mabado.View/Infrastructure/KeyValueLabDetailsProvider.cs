using Mabado.View.Domain;
using System;

namespace Mabado.View.Infrastructure
{
    internal class KeyValueLabDetailsProvider : ILabDetailsProvider
    {
        private readonly string _databaseName;
        private Func<ILabInformationProvider> _labDetailsProviderFactory;

        public KeyValueLabDetailsProvider(string databaseName, Func<ILabInformationProvider> labDetailsProviderFactory)
        {
            _databaseName = databaseName;
            _labDetailsProviderFactory = labDetailsProviderFactory;
        }

        public InstallationInfo GetDetails(ConnectionInfo connectionInfo)
        {
            string domainDbConnectionString = connectionInfo.CreateConnectionString(_databaseName);

            ILabInformationProvider labInformationProvider = _labDetailsProviderFactory();
            return labInformationProvider.GetLabInfo(domainDbConnectionString);
        }
    }
}