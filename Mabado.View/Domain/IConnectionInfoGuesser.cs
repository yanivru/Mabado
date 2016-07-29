using System.Collections.Generic;

namespace Mabado.View.Domain
{
    public interface IConnectionInfoGuesser
    {
        IEnumerable<ConnectionInfo> Guess(ConnectionInfo info);
    }
}