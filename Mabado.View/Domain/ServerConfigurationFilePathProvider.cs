using System.IO;

namespace Mabado.View.Domain
{
    internal class ServerConfigurationFilePathProvider : IPathProvider
    {
        private readonly IPathProvider _basePathProvider;

        public ServerConfigurationFilePathProvider(IPathProvider basePathProvider)
        {
            _basePathProvider = basePathProvider;
        }

        public string GetPath()
        {
            string basepath = _basePathProvider.GetPath();
            string configurationFilePath = Path.Combine(basepath, @"Server\Source\Varonis.Server.WindowsService\app.config");

            return configurationFilePath;
        }
    }
}