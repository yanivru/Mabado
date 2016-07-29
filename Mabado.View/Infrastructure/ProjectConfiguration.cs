using System.Configuration;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class ProjectConfiguration : IProjectConfiguration
    {
        private readonly Configuration _configuration;

        public ProjectConfiguration(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Save()
        {
            _configuration.Save();
        }

        public void UpdateAppSettings(string key, string value)
        {
            if (_configuration.AppSettings.Settings[key] == null)
            {
                _configuration.AppSettings.Settings.Add(key, value);
            }
            else
            {
                _configuration.AppSettings.Settings[key].Value = value;
            }
        }

        public string GetValue(string key)
        {
            KeyValueConfigurationElement keyValueConfigurationElement = _configuration.AppSettings.Settings[key];

            return keyValueConfigurationElement != null ? keyValueConfigurationElement.Value : null;
        }

        public object GetSection(string sectionName)
        {
            return _configuration.GetSection(sectionName);
        }
    }
}