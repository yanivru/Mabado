using Mabado.View.Domain;
using System;
using System.Threading.Tasks;

namespace Emulator
{
    class DummySolutionDetector : ISolutionDetector
    {
        public Task DetectAsync()
        {
            var task = new Task(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        SolutionDetected("solution " + i);
                    }
                });

            task.Start();

            return task;
        }

        public Action<string> SolutionDetected
        {
            get;
            set;
        }
    }
}
