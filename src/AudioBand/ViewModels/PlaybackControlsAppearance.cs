using AudioBand.Annotations;
using Svg;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class PlayPauseButtonAppearance : INotifyPropertyChanged
    {
        private static readonly SvgDocument DefaultPlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play)).SizeFix();
        private static readonly SvgDocument DefaultPauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private Image _playImage;
        private string _playImagePath = "";
        private Image _pauseImage;
        private string _pauseImagePath = "";
        private int _yPosition = 14;
        private int _xPosition = 103;
        private int _height = 12;
        private int _width = 73;
        private bool _isVisible = true;
        private bool _isPlaying;

        public Image PlayImage
        {
            get => _playImage;
            set
            {
                if (Equals(value, _playImage)) return;
                _playImage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
            }
        }

        public string PlayImagePath
        {
            get => _playImagePath;
            set
            {
                _playImagePath = value;
                OnPropertyChanged();
                LoadPlayImage();
            }
        }

        public Image PauseImage
        {
            get => _pauseImage;
            set
            {
                if (Equals(value, _pauseImage)) return;
                _pauseImage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
            }
        }

        public string PauseImagePath
        {
            get => _pauseImagePath;
            set
            {
                _pauseImagePath = value;
                LoadPauseImage();
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
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
                OnPropertyChanged(nameof(Image));
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Image Image
        {
            get
            {
                var image = _isPlaying ? PlayImage : PauseImage;
                return image.Scale(Width, Height);
            }
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PlayPauseButtonAppearance()
        {
            LoadPlayImage();
            LoadPauseImage();
        }

        private void LoadPauseImage()
        {
            try
            {
                PauseImage = string.IsNullOrEmpty(PauseImagePath) ? DefaultPauseButtonSvg.ToBitmap() : Image.FromFile(_pauseImagePath);
            }
            catch (Exception e)
            {
                PauseImage = DefaultPauseButtonSvg.ToBitmap();
            }
        }

        private void LoadPlayImage()
        {
            try
            {
                PlayImage = string.IsNullOrEmpty(_playImagePath) ? DefaultPlayButtonSvg.ToBitmap() : Image.FromFile(_playImagePath);
            }
            catch (Exception e)
            {
                PlayImage = DefaultPlayButtonSvg.ToBitmap();
            }
        }
    }

    internal class NextSongButtonAppearance : INotifyPropertyChanged
    {
        private static readonly SvgDocument DefaultNextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private Image _image;
        private string _imagePath = "";
        private int _yPosition = 14;
        private int _xPosition = 176;
        private int _height = 12;
        private int _width = 73;
        private bool _isVisible = true;

        public Image Image
        {
            get => _image.Scale(Width, Height);
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
                LoadImage();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
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
                OnPropertyChanged(nameof(Image));
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NextSongButtonAppearance()
        {
            LoadImage();
        }

        private void LoadImage()
        {
            try
            {
                Image = string.IsNullOrEmpty(_imagePath) ? DefaultNextButtonSvg.ToBitmap() : Image.FromFile(_imagePath);
            }
            catch (Exception e)
            {
                Image = DefaultNextButtonSvg.ToBitmap();
            }
        }
    }

    internal class PreviousSongButtonAppearance : INotifyPropertyChanged
    {
        private static readonly SvgDocument DefaultPreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous)).SizeFix();
        private Image _image;
        private string _imagePath = "";
        private bool _isVisible = true;
        private int _width = 73;
        private int _height = 12;
        private int _xPosition = 30;
        private int _yPosition = 14;

        public Image Image
        {
            get => _image.Scale(Width, Height);
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
                LoadImage();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
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
                OnPropertyChanged(nameof(Image));
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PreviousSongButtonAppearance()
        {
            LoadImage();
        }

        private void LoadImage()
        {
            try
            {
                Image = string.IsNullOrEmpty(_imagePath) ? DefaultPreviousButtonSvg.ToBitmap() : Image.FromFile(_imagePath);
            }
            catch (Exception e)
            {
                Image = DefaultPreviousButtonSvg.ToBitmap();
            }
        }
    }
}
