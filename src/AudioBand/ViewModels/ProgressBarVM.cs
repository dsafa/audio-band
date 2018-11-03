using System;
using AudioBand.Models;
using System.Drawing;

namespace AudioBand.ViewModels
{
    internal class ProgressBarVM : ViewModelBase<ProgressBar>
    {
        private readonly Track _track;

        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetModelProperty(nameof(Model.ForegroundColor), value);
        }

        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetModelProperty(nameof(Model.BackgroundColor), value);
        }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelProperty(nameof(Model.IsVisible), value);
        }

        public int Width
        {
            get => Model.Width;
            set => SetModelProperty(nameof(Model.Width), value);
        }

        public int Height
        {
            get => Model.Height;
            set => SetModelProperty(nameof(Model.Height), value);
        }

        public int XPosition
        {
            get => Model.XPosition;
            set => SetModelProperty(nameof(Model.XPosition), value, alsoNotify: nameof(Location));
        }

        public int YPosition
        {
            get => Model.YPosition;
            set => SetModelProperty(nameof(Model.YPosition), value, alsoNotify: nameof(Location));
        }

        public TimeSpan TrackProgress => _track.TrackProgress;

        public TimeSpan TrackLength => _track.TrackLength;

        public Point Location => new Point(Model.XPosition, Model.YPosition);

        public ProgressBarVM(ProgressBar model, Track track) : base(model)
        {
            _track = track;
        }
    }
}
