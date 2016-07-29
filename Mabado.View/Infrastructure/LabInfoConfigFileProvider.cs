using System.Globalization;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class LabInfoConfigFileProvider : ILabInfoProvider
    {
        private readonly IConfigurationProvider _configProvider;

        public LabInfoConfigFileProvider(IConfigurationProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void Update(InstallationInfo installationInfo)
        {
            IProjectConfiguration config = _configProvider.GetConfig();

            config.UpdateAppSettings("IMPERSONATION_SIDID", installationInfo.UserSid.ToString(CultureInfo.InvariantCulture));
            config.UpdateAppSettings("ProductVersion", installationInfo.Version);

            config.Save();
        }

        public InstallationInfo Read()
        {
            IProjectConfiguration config = _configProvider.GetConfig();

            string impersonationSidId = config.GetValue("IMPERSONATION_SIDID");

            InstallationInfo installationInfo = new InstallationInfo();
            installationInfo.UserSid = string.IsNullOrEmpty(impersonationSidId)? 0: int.Parse(impersonationSidId);
            installationInfo.Version = config.GetValue("ProductVersion");

            return installationInfo;
        }
    }
}