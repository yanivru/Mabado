using System;
using System.Collections.Generic;
using System.Linq;

namespace Mabado.View.Domain
{
    internal class HardcodedSolutionPriorityResolver : ISolutionPriorityResolver
    {
        public int Resolve(string fullFilePath)
        {
            var solutionsPriorities = new List<Tuple<string, int>>();
            solutionsPriorities.Add(new Tuple<string, int>(@"Client\Source\DataVantage", 2));
            solutionsPriorities.Add(new Tuple<string, int>(@"Server\Source\IduServer", 2));
            solutionsPriorities.Add(new Tuple<string, int>(@"ManagementConsole\Source\ManagementConsole.sln", 2));
            solutionsPriorities.Add(new Tuple<string, int>(@"Installer\Source\Installer.sln", 2));
            solutionsPriorities.Add(new Tuple<string, int>(@"Client\Source\ReportsDeployment2.sln", 1));

            Tuple<string, int> solutionPriority = solutionsPriorities.FirstOrDefault(x => fullFilePath.Contains(x.Item1));

            return solutionPriority == null ? 0 : solutionPriority.Item2;
        }
    }
}