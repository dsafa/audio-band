using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;
using AudioBand.TextFormatting;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

#pragma warning disable 1591

namespace AudioBand.ViewModels
{
    /// <summary>
    /// The view model for a custom label.
    /// </summary>
    public class CustomLabelVM : ViewModelBase<CustomLabel>
    {
        private readonly FormattedTextRenderer _renderer;
        private IAudioSource _audioSource;
        private bool _isPlaying;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelVM"/> class.
        /// </summary>
        /// <param name="model">The custom label.</param>
        /// <param name="dialogService">The dialog service.</param>
        public CustomLabelVM(CustomLabel model, IDialogService dialogService)
            : base(model)
        {
            DialogService = dialogService;
            _renderer = new FormattedTextRenderer(FormatString, Color);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Name))]
        public string Name
        {
            get => Model.Name;
            set => SetProperty(nameof(Model.Name), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FontFamily))]
        public string FontFamily
        {
            get => Model.FontFamily;
            set => SetProperty(nameof(Model.FontFamily), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FontSize))]
        public float FontSize
        {
            get => Model.FontSize;
            set => SetProperty(nameof(Model.FontSize), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Color))]
        public Color Color
        {
            get => Model.Color;
            set => SetProperty(nameof(Model.Color), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FormatString))]
        [AlsoNotify(nameof(TextSegments))]
        public string FormatString
        {
            get => Model.FormatString;
            set => SetProperty(nameof(Model.FormatString), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Alignment))]
        public TextAlignment TextAlignment
        {
            get => Model.Alignment;
            set => SetProperty(nameof(Model.Alignment), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Width))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Height))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.XPosition))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.YPosition))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.ScrollSpeed))]
        public TimeSpan ScrollSpeed
        {
            get => TimeSpan.FromMilliseconds(Model.ScrollSpeed);
            set => SetProperty(nameof(Model.ScrollSpeed), (int)value.TotalMilliseconds);
        }

        /// <summary>
        /// Gets or sets the text overflow.
        /// </summary>
        [PropertyChangeBinding(nameof(CustomLabel.TextOverflow))]
        public TextOverflow TextOverflow
        {
            get => Model.TextOverflow;
            set => SetProperty(nameof(Model.TextOverflow), value);
        }

        /// <summary>
        /// Gets or sets the scroll behavior.
        /// </summary>
        [PropertyChangeBinding(nameof(CustomLabel.ScrollBehavior))]
        public ScrollBehavior ScrollBehavior
        {
            get => Model.ScrollBehavior;
            set => SetProperty(nameof(Model.ScrollBehavior), value);
        }

        /// <summary>
        /// Gets the text segments.
        /// </summary>
        public IEnumerable<TextSegment> TextSegments => _renderer.TextSegments;

        /// <summary>
        /// Gets the values of <see cref="CustomLabel.TextAlignment"/>.
        /// </summary>
        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();

        /// <summary>
        /// Gets the values of the <see cref="ScrollBehavior"/> enum.
        /// </summary>
        public IEnumerable<EnumDescriptor<ScrollBehavior>> ScrollBehaviorValues { get; } = typeof(ScrollBehavior).GetEnumDescriptors<ScrollBehavior>();

        /// <summary>
        /// Gets the values of the <see cref="TextOverflow"/> enum.
        /// </summary>
        public IEnumerable<EnumDescriptor<TextOverflow>> TextOverflowValues { get; } = typeof(TextOverflow).GetEnumDescriptors<TextOverflow>();

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether a track is playing.
        /// </summary>
        /// <remarks>Public so that bindings are set up correctly.</remarks>
        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value, false);
        }

        /// <inheritdoc/>
        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.Color):
                    _renderer.DefaultColor = Model.Color;
                    break;
                case nameof(Model.FormatString):
                    _renderer.Format = Model.FormatString;
                    break;
            }
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                Clear();
                _audioSource.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
                _audioSource.TrackProgressChanged -= AudioSourceOnTrackProgressChanged;
                _audioSource.IsPlayingChanged -= AudioSourceOnIsPlayingChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                Clear();
                return;
            }

            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
            _audioSource.IsPlayingChanged += AudioSourceOnIsPlayingChanged;
        }

        private void AudioSourceOnIsPlayingChanged(object sender, bool e)
        {
            IsPlaying = e;
        }

        private void AudioSourceOnTrackProgressChanged(object sender, TimeSpan e)
        {
            _renderer.SongProgress = e;
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            _renderer.Artist = e.Artist;
            _renderer.AlbumName = e.Album;
            _renderer.SongLength = e.TrackLength;
            _renderer.SongName = e.TrackName;
        }

        private void Clear()
        {
            _renderer.Artist = null;
            _renderer.AlbumName = null;
            _renderer.SongName = null;
            _renderer.SongLength = TimeSpan.Zero;
            _renderer.SongProgress = TimeSpan.Zero;
        }
    }
}
#pragma warning restore 1591
