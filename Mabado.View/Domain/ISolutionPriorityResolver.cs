namespace Mabado.View.Domain
{
    internal interface ISolutionPriorityResolver
    {
        int Resolve(string fullFilePath);
    }
}