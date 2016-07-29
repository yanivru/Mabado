using System;
using System.IO;
using EnvDTE80;
using Mabado.View.Domain;

namespace Infrastructre
{
    /// <summary>
    /// Detects the base directory of the branch by the current solution path
    /// </summary>
    public class BranchPathProvider : IPathProvider
    {
        private readonly DTE2 _applicationObject;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="applicationObject"></param>
        public BranchPathProvider(DTE2 applicationObject)
        {
            _applicationObject = applicationObject;
        }

        /// <summary>
        /// Returns the path
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetPath()
        {
            string path = Path.GetDirectoryName(_applicationObject.Solution.FileName);
            
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Cannot locate server solution. Path is null.");
            }

            DirectoryInfo directory = new DirectoryInfo(path);

            if (directory.Parent == null || directory.Parent.Parent == null)
            {
                throw new Exception("Provided path is not a base directory: " + path);
            }

            string baseDirectory = directory.Parent.Parent.FullName;

            if (!directory.Parent.Parent.Exists)
            {
                throw new Exception(string.Format("Base directory {0} does not exists", baseDirectory));
            }

            return baseDirectory;
        }
    }
}