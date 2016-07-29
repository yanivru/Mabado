using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mabado.View.Annotations;

namespace Mabado.View.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly object _contentViewModel;
        private string _subtitle;

        public object ContentViewModel
        {
            get
            {
                return _contentViewModel;
            }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                if (value == _subtitle) return;
                _subtitle = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(object contentViewModel, string subtitle)
        {
            _contentViewModel = contentViewModel;
            Subtitle = subtitle;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
