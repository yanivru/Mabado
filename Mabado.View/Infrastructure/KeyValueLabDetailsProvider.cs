using Mabado.View.Commands;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class KeyValueLabDetailsProvider : ILabDetailsProvider
    {
        private readonly string _databaseName;

        public KeyValueLabDetailsProvider(string databaseName)
        {
            _databaseName = databaseName;
        }

        public InstallationInfo GetDetails(ConnectionInfo connectionInfo)
        {
            string domainDbConnectionString = connectionInfo.CreateConnectionString(_databaseName);

            LabInformationProvider labInformationProvider = new LabInformationProvider();
            return labInformationProvider.GetLabInfo(domainDbConnectionString);
        }
    }
}