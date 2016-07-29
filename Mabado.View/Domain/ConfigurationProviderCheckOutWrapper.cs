namespace Mabado.View.Domain
{
    internal class ConfigurationProviderCheckOutWrapper : IConfigurationProvider
    {
        private readonly IConfigurationProvider _configProvider;
        private readonly IPathProvider _configPathProvider;
        private readonly ISourceControl _sourceControl;

        public ConfigurationProviderCheckOutWrapper(IConfigurationProvider configProvider, IPathProvider configPathProvider, ISourceControl sourceControl)
        {
            _configProvider = configProvider;
            _configPathProvider = configPathProvider;
            _sourceControl = sourceControl;
        }

        public IProjectConfiguration GetConfig()
        {
            return new ConfigCheckOutWrapper(_configProvider.GetConfig(), _configPathProvider.GetPath(), _sourceControl);
        }
    }
}