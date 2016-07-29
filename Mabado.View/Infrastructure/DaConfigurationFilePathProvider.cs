using System.IO;
using Mabado.View.Domain;

namespace Mabado.View.Factories
{
    internal class DaConfigurationFilePathProvider : IPathProvider
    {
        private readonly IPathProvider _basePathProvider;

        public DaConfigurationFilePathProvider(IPathProvider basePathProvider)
        {
            _basePathProvider = basePathProvider;
        }

        public string GetPath()
        {
            string basePath = _basePathProvider.GetPath();
            return Path.Combine(basePath, @"Client\Source\Windows\DataVantage.Windows\app.config");
        }
    }
}