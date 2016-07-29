namespace Mabado.View.Domain
{
    internal interface IProjectConfiguration
    {
        void Save();
        void UpdateAppSettings(string key, string value);
        string GetValue(string key);
        object GetSection(string sectionName);
    }
}