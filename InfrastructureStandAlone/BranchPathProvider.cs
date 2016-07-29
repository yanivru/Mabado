using Mabado.View.Domain;
using System.IO;

namespace InfrastructureStandAlone
{
    public class BranchPathProvider : IPathProvider
    {
        public string GetPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            return currentDirectory.Parent.Parent.FullName;
        }
    }
}