namespace Mabado.View.Domain
{
    public interface ISolutionConnectionStrings
    {
        void Update(ConnectionInfo connectionInfo);
        ConnectionInfo Read();
    }
}