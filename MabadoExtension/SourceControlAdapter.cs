using EnvDTE;
using Mabado.View.Domain;

namespace Reopened.MabadoExtension
{
    internal class SourceControlAdapter : ISourceControl
    {
        private readonly SourceControl _sourceControl;

        public SourceControlAdapter(SourceControl sourceControl)
        {
            _sourceControl = sourceControl;
        }

        public bool CheckOutItem(string itemName)
        {
            return _sourceControl.CheckOutItem(itemName);
        }
    }
}