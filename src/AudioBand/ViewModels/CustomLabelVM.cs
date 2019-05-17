using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly FormattedTextParser _parser;
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
            _parser = new FormattedTextParser(FormatString, Color);
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
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.YPosition))]
        public double YPosition
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
        public IEnumerable<TextSegment> TextSegments => _parser.TextSegments;

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
                    _parser.DefaultColor = Model.Color;
                    break;
                case nameof(Model.FormatString):
                    _parser.Format = Model.FormatString;
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
            _parser.SongProgress = e;
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            _parser.Artist = e.Artist;
            _parser.AlbumName = e.Album;
            _parser.SongLength = e.TrackLength;
            _parser.SongName = e.TrackName;
        }

        private void Clear()
        {
            _parser.Artist = null;
            _parser.AlbumName = null;
            _parser.SongName = null;
            _parser.SongLength = TimeSpan.Zero;
            _parser.SongProgress = TimeSpan.Zero;
        }
    }
}
#pragma warning restore 1591
