using AudioBand.Annotations;
using Svg;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using NLog;

namespace AudioBand.ViewModels
{
    internal class PlayPauseButtonAppearance : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private static readonly SvgDocument DefaultPlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private static readonly SvgDocument DefaultPauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private Image _playImage;
        private Image _pauseImage;
        private bool _isPlaying;

        private string _playImagePath;
        private string _pauseImagePath;
        private int _yPosition;
        private int _xPosition;
        private int _height;
        private int _width;
        private bool _isVisible;

        private PlayPauseButtonAppearance _backup;

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
                PlayImage = LoadImage(value, DefaultPlayButtonSvg.ToBitmap());
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
                OnPropertyChanged();
                PauseImage = LoadImage(value, DefaultPauseButtonSvg.ToBitmap());
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
                var image = _isPlaying ? PauseImage : PlayImage;
                return image.Scale(Width - 1, Height - 1); // To fit image in button
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

        public PlayPauseButtonAppearance()
        {
            Reset();
        }

        private Image LoadImage(string path, Image defaultImage)
        {
            try
            {
                return string.IsNullOrEmpty(path) ? defaultImage : Image.FromFile(path);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Debug($"Error loading image from {path}, {e}");
                return defaultImage;
            }
        }

        public void BeginEdit()
        {
            _backup = new PlayPauseButtonAppearance
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XPosition = XPosition,
                YPosition = YPosition,
                PlayImagePath = PlayImagePath == null ? null : string.Copy(PlayImagePath),
                PauseImagePath = PauseImagePath == null ? null : string.Copy(PauseImagePath),
            };
        }

        public void EndEdit()
        {
            // No op
        }

        public void CancelEdit()
        {
            IsVisible = _backup.IsVisible;
            Width = _backup.Width;
            Height = _backup.Height;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            PlayImagePath = _backup.PlayImagePath;
            PauseImagePath = _backup.PauseImagePath;
        }

        public void Reset()
        {
            PlayImagePath = "";
            PauseImagePath = "";
            YPosition = 15;
            XPosition = 103;
            Height = 12;
            Width = 73;
            IsVisible = true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class NextSongButtonAppearance : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private static readonly SvgDocument DefaultNextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private Image _image;

        private string _imagePath;
        private int _yPosition;
        private int _xPosition;
        private int _height;
        private int _width;
        private bool _isVisible;

        private NextSongButtonAppearance _backup;

        public Image Image
        {
            get => _image.Scale(Width - 1, Height - 1);
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

        public NextSongButtonAppearance()
        {
            Reset();
        }

        private void LoadImage()
        {
            try
            {
                Image = string.IsNullOrEmpty(_imagePath) ? DefaultNextButtonSvg.ToBitmap() : Image.FromFile(_imagePath);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Debug($"Error loading image from {ImagePath}, {e}");
                Image = DefaultNextButtonSvg.ToBitmap();
            }
        }

        public void BeginEdit()
        {
            _backup = new NextSongButtonAppearance
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XPosition = XPosition,
                YPosition = YPosition,
                ImagePath = ImagePath == null ? null : string.Copy(ImagePath)
            };
        }

        public void EndEdit()
        {
            // No op
        }

        public void CancelEdit()
        {
            IsVisible = _backup.IsVisible;
            Width = _backup.Width;
            Height = _backup.Height;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            ImagePath = _backup.ImagePath;
        }

        public void Reset()
        {
            ImagePath = "";
            YPosition = 15;
            XPosition = 176;
            Height = 12;
            Width = 73;
            IsVisible = true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class PreviousSongButtonAppearance : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private static readonly SvgDocument DefaultPreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private Image _image;

        private string _imagePath;
        private bool _isVisible;
        private int _width;
        private int _height;
        private int _xPosition;
        private int _yPosition;

        private PreviousSongButtonAppearance _backup;

        public Image Image
        {
            get => _image.Scale(Width - 1, Height - 1);
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

        public PreviousSongButtonAppearance()
        {
            Reset();
        }

        private void LoadImage()
        {
            try
            {
                Image = string.IsNullOrEmpty(_imagePath) ? DefaultPreviousButtonSvg.ToBitmap() : Image.FromFile(_imagePath);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Debug($"Error loading image from {ImagePath}, {e}");
                Image = DefaultPreviousButtonSvg.ToBitmap();
            }
        }

        public void BeginEdit()
        {
            _backup = new PreviousSongButtonAppearance
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XPosition = XPosition,
                YPosition = YPosition,
                ImagePath = ImagePath == null ? null : string.Copy(ImagePath)
            };
        }

        public void EndEdit()
        {
            // No op
        }

        public void CancelEdit()
        {
            IsVisible = _backup.IsVisible;
            Width = _backup.Width;
            Height = _backup.Height;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            ImagePath = _backup.ImagePath;
        }

        public void Reset()
        {
            ImagePath = "";
            IsVisible = true;
            Width = 73;
            Height = 12;
            XPosition = 30;
            YPosition = 15;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
