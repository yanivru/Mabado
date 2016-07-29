namespace Mabado.View.Domain
{
    internal class ConfigCheckOutWrapper : IProjectConfiguration
    {
        private readonly IProjectConfiguration _config;
        private readonly string _path;
        private readonly ISourceControl _sourceControl;

        public ConfigCheckOutWrapper(IProjectConfiguration config, string path, ISourceControl sourceControl)
        {
            _config = config;
            _path = path;
            _sourceControl = sourceControl;
        }

        public void Save()
        {
            _sourceControl.CheckOutItem(_path);
            _config.Save();
        }
        
        public void UpdateAppSettings(string key, string value)
        {
            _config.UpdateAppSettings(key, value);
        }
        
        public string GetValue(string key)
        {
            return _config.GetValue(key);
        }

        public object GetSection(string sectionName)
        {
            return _config.GetSection(sectionName);
        }
    }
}