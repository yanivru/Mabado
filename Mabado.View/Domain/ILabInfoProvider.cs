namespace Mabado.View.Domain
{
    public interface ILabInfoProvider
    {
        void Update(InstallationInfo installationInfo);
        InstallationInfo Read();
    }
}