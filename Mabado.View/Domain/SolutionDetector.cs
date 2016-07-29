using System;
using System.IO;
using System.Threading.Tasks;
using Mabado.View.Domain;

namespace Mabado.View.Commands
{
    internal class SolutionDetector : ISolutionDetector
    {
        private readonly IPathProvider _basePathProvider;
        private string _basepath;

        public SolutionDetector(IPathProvider basePathProvider)
        {
            _basePathProvider = basePathProvider;
        }

        public Task DetectAsync()
        {
            _basepath = _basePathProvider.GetPath();

            Task task = new Task(Action);
            task.Start();

            return task;
        }

        public Action<string> SolutionDetected { get; set; }

        private void Action()
        {
            DirectoryInfo baseDir = new DirectoryInfo(_basepath);
            foreach (var fileInfo in baseDir.GetFiles("*.sln", SearchOption.AllDirectories))
            {
                SolutionDetected?.Invoke(fileInfo.FullName);
            }
        }
    }
}