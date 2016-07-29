using System.IO;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class McConfigurationFilePathProvider : IPathProvider
    {
        private readonly IPathProvider _basePathProvider;

        public McConfigurationFilePathProvider(IPathProvider basePathProvider)
        {
            _basePathProvider = basePathProvider;
        }

        public string GetPath()
        {
            string basePath = _basePathProvider.GetPath();
            return Path.Combine(basePath, @"ManagementConsole\Source\Varonis.UI.ManagementConsole.Shell\App.config");
        }
    }
}