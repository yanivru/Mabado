using System.Configuration;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class SolutionConnectionStrings : ISolutionConnectionStrings
    {
        private readonly IConfigurationProvider _configProvider;

        public SolutionConnectionStrings(IConfigurationProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void Update(ConnectionInfo connectionInfo)
        {
            IProjectConfiguration projectConfiguration = _configProvider.GetConfig();

            var connectionStringsSection = (ConnectionStringsSection)projectConfiguration.GetSection("connectionStrings");

            foreach (ConnectionStringSettings connectionStringSettings in connectionStringsSection.ConnectionStrings)
            {
                string updatedConnectionString = connectionInfo.UpdateConnectionString(connectionStringSettings.ConnectionString);
                connectionStringSettings.ConnectionString = updatedConnectionString;                
            }

            projectConfiguration.Save();
        }

        public ConnectionInfo Read()
        {
            IProjectConfiguration projectConfiguration = _configProvider.GetConfig();

            var connectionStringsSection = (ConnectionStringsSection)projectConfiguration.GetSection("connectionStrings");

            if (connectionStringsSection.ConnectionStrings.Count == 0)
            {
                return new ConnectionInfo {Host = "", Instance = "", UserName = "", Password = ""};
            }
            
            return ConnectionInfo.FromConnectionString(connectionStringsSection.ConnectionStrings[0].ConnectionString);
        }
    }
}