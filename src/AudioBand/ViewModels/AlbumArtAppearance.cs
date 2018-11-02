﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;
using AudioBand.Extensions;
using NLog;
using Svg;

namespace AudioBand.ViewModels
{
    internal class AlbumArtDisplay : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private bool _isVisible;
        private int _width;
        private int _height;
        private int _xPosition;
        private int _yPosition;
        private string _placeholderPath;

        private Image _placeholder;
        private Image _currentAlbumArt = new Bitmap(1, 1);
        private static readonly SvgDocument DefaultAlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));

        private AlbumArtDisplay _backup;

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
                OnPropertyChanged(nameof(CurrentAlbumArt));
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
                OnPropertyChanged(nameof(CurrentAlbumArt));
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

        public Image Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                OnPropertyChanged();
            }
        }

        public string PlaceholderPath
        {
            get => _placeholderPath;
            set
            {
                _placeholderPath = value;
                OnPropertyChanged();
                LoadPlaceholder();
            }
        }

        public Image CurrentAlbumArt
        {
            get => _currentAlbumArt.Resize(Width, Height);
            set
            {
                if (value == _currentAlbumArt) return;
                _currentAlbumArt = value;
                OnPropertyChanged();
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        public AlbumArtDisplay()
        {
            Reset();
        }

        private void LoadPlaceholder()
        {
            try
            {
                Placeholder = string.IsNullOrEmpty(_placeholderPath) ? DefaultAlbumArtPlaceholderSvg.ToBitmap() : Image.FromFile(_placeholderPath);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Debug($"Error loading image from {_placeholderPath}, {e}");
                Placeholder = DefaultAlbumArtPlaceholderSvg.ToBitmap();
            }
        }

        public void BeginEdit()
        {
            _backup = new AlbumArtDisplay
            {
                Height = Height,
                Width = Width,
                YPosition = YPosition,
                XPosition = XPosition,
                IsVisible = IsVisible,
                PlaceholderPath = PlaceholderPath == null ? null : string.Copy(PlaceholderPath),
            };
        }

        public void EndEdit()
        {
            // No op
        }

        public void CancelEdit()
        {
            Height = _backup.Height;
            Width = _backup.Width;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            PlaceholderPath = _backup.PlaceholderPath;
            IsVisible = _backup.IsVisible;
        }

        public void Reset()
        {
            IsVisible = true;
            Width = 30;
            Height = 30;
            XPosition = 0;
            YPosition = 0;
            PlaceholderPath = "";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class AlbumArtPopup : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private bool _isVisible;
        private int _width;
        private int _height;
        private int _xOffset;
        private int _margin;

        private Image _currentAlbumArt = new Bitmap(1, 1);

        private AlbumArtPopup _backup;

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

        public int XOffset
        {
            get => _xOffset;
            set
            {
                if (value == _xOffset) return;
                _xOffset = value;
                OnPropertyChanged();
            }
        }

        public int Margin
        {
            get => _margin;
            set
            {
                if (value == _margin) return;
                _margin = value;
                OnPropertyChanged();
            }
        }

        public Image CurrentAlbumArt
        {
            get => _currentAlbumArt.Resize(Width, Height);
            set
            {
                if (value == _currentAlbumArt) return;
                _currentAlbumArt = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AlbumArtPopup()
        {
            Reset();
        }

        public void BeginEdit()
        {
            _backup = new AlbumArtPopup
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XOffset = XOffset,
                Margin = Margin
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
            XOffset = _backup.XOffset;
            Margin = _backup.Margin;
        }

        public void Reset()
        {
            IsVisible = true;
            Width = 250;
            Height = 250;
            XOffset = 0;
            Margin = 4;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
