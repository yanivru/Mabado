using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    class SolutionLauncher : ISolutionLauncher
    {
        public void OpenSolution(string fullFilePath)
        {
            System.Diagnostics.Process.Start(fullFilePath);
        }
    }
}
