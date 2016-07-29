using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Mabado.View.Annotations;

namespace Mabado.View.ViewModels
{
    public class SolutionViewModel : INotifyPropertyChanged
    {
        private string _fullFilePath;

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(FullFilePath); }
        }

        public string FullFilePath
        {
            get { return _fullFilePath; }
            set
            {
                if (value == _fullFilePath) return;
                _fullFilePath = value;
                OnPropertyChanged();
                OnPropertyChanged("Name");
            }
        }

        public int Priority { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            var vm = obj as SolutionViewModel;

            return vm != null && FullFilePath == vm.FullFilePath && Priority == vm.Priority;
        }

        public override int GetHashCode()
        {
            return FullFilePath.GetHashCode();
        }
    }
}