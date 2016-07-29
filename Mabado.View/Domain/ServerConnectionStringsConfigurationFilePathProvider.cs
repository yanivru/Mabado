using System.IO;

namespace Mabado.View.Domain
{
    internal class ServerConnectionStringsConfigurationFilePathProvider : IPathProvider
    {
        private readonly IPathProvider _basePathProvider;

        public ServerConnectionStringsConfigurationFilePathProvider(IPathProvider basePathProvider)
        {
            _basePathProvider = basePathProvider;
        }

        public string GetPath()
        {
            string basepath = _basePathProvider.GetPath();
            string configurationFilePath = Path.Combine(basepath, @"Server\Source\Varonis.Server.WindowsService\ConnectionStrings.config");

            return configurationFilePath;
        }
    }
}