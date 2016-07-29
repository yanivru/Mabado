using System;
using EnvDTE;
using EnvDTE80;
using Mabado.View.Domain;

namespace Infrastructre
{
    public class OutputWindowLogger : ILogger
    {
        public const string VsWindowKindOutput = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
        private readonly Lazy<OutputWindowPane> _mabadoPane;

        private OutputWindowPane Pane
        {
            get { return _mabadoPane.Value; }
        }

        public OutputWindowLogger(DTE2 applicationObject)
        {
            _mabadoPane = new Lazy<OutputWindowPane>(() => GetPane(applicationObject, "Mabado"));
        }

        private OutputWindowPane GetPane(DTE2 applicationObject, string paneName)
        {
            Window window = applicationObject.Windows.Item(VsWindowKindOutput);
            OutputWindow outputWindow = (OutputWindow)window.Object;

            for (uint i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
            {
                if (outputWindow.OutputWindowPanes.Item(i).Name.Equals(paneName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return outputWindow.OutputWindowPanes.Item(i);
                }
            }

            return outputWindow.OutputWindowPanes.Add(paneName);
        }

        public void WriteError(Exception exception)
        {
            Pane.Activate();
            Pane.OutputString(exception.ToString());
        }

        public void WriteInfo(string message)
        {
            Pane.Activate();
            Pane.OutputString(message);
        }
    }
}