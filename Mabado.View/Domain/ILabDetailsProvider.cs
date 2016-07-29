namespace Mabado.View.Domain
{
    public interface ILabDetailsProvider
    {
        InstallationInfo GetDetails(ConnectionInfo connectionInfo);
    }
}