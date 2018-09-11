using System.ComponentModel;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class AudioBandAppearance : INotifyPropertyChanged, IEditableObject
    {
        private int _width = 250;
        private int _height = 30;

        private AudioBandAppearance _backup;

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginEdit()
        {
            _backup = new AudioBandAppearance
            {
                Width = Width,
                Height = Height
            };
        }

        public void EndEdit()
        {

        }

        public void CancelEdit()
        {
            Width = _backup.Width;
            Height = _backup.Height;
        }
    }
}
