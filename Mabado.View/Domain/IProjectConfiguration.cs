namespace Mabado.View.Domain
{
    public interface IProjectConfiguration
    {
        void Save();
        void UpdateAppSettings(string key, string value);
        string GetValue(string key);
        object GetSection(string sectionName);
    }
}