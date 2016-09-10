namespace Mabado.View.Domain
{
    public interface ILabInformationProvider
    {
        InstallationInfo GetLabInfo(string domainDbConnectionString);
    }
}