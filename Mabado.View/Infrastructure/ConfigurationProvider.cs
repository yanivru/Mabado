using System.Configuration;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IPathProvider _configPathProvider;

        public ConfigurationProvider(IPathProvider configPathProvider)
        {
            _configPathProvider = configPathProvider;
        }

        public IProjectConfiguration GetConfig()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = _configPathProvider.GetPath();
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            
            ProjectConfiguration projectConfiguration = new ProjectConfiguration(config);
            return projectConfiguration;
        }
    }
}