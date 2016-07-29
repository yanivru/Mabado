using Mabado.View.Infrastructure;

namespace Mabado.View.Domain
{
    internal interface IConfigurationProvider
    {
        IProjectConfiguration GetConfig();
    }
}