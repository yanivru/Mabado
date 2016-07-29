using System;
using System.Threading.Tasks;

namespace Mabado.View.Domain
{
    public interface ISolutionDetector
    {
        Task DetectAsync();
        Action<string> SolutionDetected { get; set; }
    }
}