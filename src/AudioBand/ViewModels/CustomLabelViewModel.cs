using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.TextFormatting;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// The view model for a custom label.
    /// </summary>
    public class CustomLabelViewModel : LayoutViewModelBase<CustomLabel>
    {
        private readonly CustomLabel _source;
        private readonly IAudioSession _audioSession;
        private bool _isPlaying;
        private IEnumerable<TextSegment> _textSegments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelViewModel"/> class.
        /// </summary>
        /// <param name="source">The custom label.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public CustomLabelViewModel(CustomLabel source, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(messageBus, source)
        {
            _source = source;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;

            DialogService = dialogService;
            TextSegments = FormattedTextParser.ParseFormattedString(FormatString, Color, audioSession);
            IsPlaying = _audioSession.IsPlaying;
        }

        /// <summary>
        /// Gets or sets the name of the label.
        /// </summary>
        [TrackState]
        public string Name
        {
            get => Model.Name;
            set => SetProperty(Model, nameof(Model.Name), value);
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        [TrackState]
        public string FontFamily
        {
            get => Model.FontFamily;
            set => SetProperty(Model, nameof(Model.FontFamily), value);
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        [TrackState]
        public float FontSize
        {
            get => Model.FontSize;
            set => SetProperty(Model, nameof(Model.FontSize), value);
        }

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        [TrackState]
        public Color Color
        {
            get => Model.Color;
            set => SetProperty(Model, nameof(Model.Color), value);
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        [AlsoNotify(nameof(TextSegments))]
        [TrackState]
        public string FormatString
        {
            get => Model.FormatString;
            set => SetProperty(Model, nameof(Model.FormatString), value);
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        [TrackState]
        public TextAlignment TextAlignment
        {
            get => Model.Alignment;
            set => SetProperty(Model, nameof(Model.Alignment), value);
        }

        /// <summary>
        /// Gets or sets the scroll speed.
        /// </summary>
        [TrackState]
        public TimeSpan ScrollSpeed
        {
            get => TimeSpan.FromMilliseconds(Model.ScrollSpeed);
            set => SetProperty(Model, nameof(Model.ScrollSpeed), (int)value.TotalMilliseconds);
        }

        /// <summary>
        /// Gets or sets the text overflow.
        /// </summary>
        [TrackState]
        public TextOverflow TextOverflow
        {
            get => Model.TextOverflow;
            set => SetProperty(Model, nameof(Model.TextOverflow), value);
        }

        /// <summary>
        /// Gets or sets the scroll behavior.
        /// </summary>
        [TrackState]
        public ScrollBehavior ScrollBehavior
        {
            get => Model.ScrollBehavior;
            set => SetProperty(Model, nameof(Model.ScrollBehavior), value);
        }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        [TrackState]
        public TextFadeEffect FadeEffect
        {
            get => Model.FadeEffect;
            set => SetProperty(Model, nameof(Model.FadeEffect), value);
        }

        /// <summary>
        /// Gets or sets the left offset for the text fade gradient.
        /// </summary>
        [TrackState]
        public double LeftFadeOffset
        {
            get => Model.LeftFadeOffset;
            set => SetProperty(Model, nameof(Model.LeftFadeOffset), value);
        }

        /// <summary>
        /// Gets or sets the right offset for the text fade gradient.
        /// </summary>
        [TrackState]
        public double RightFadeOffset
        {
            get => Model.RightFadeOffset;
            set => SetProperty(Model, nameof(Model.RightFadeOffset), value);
        }

        /// <summary>
        /// Gets the text segments.
        /// </summary>
        public IEnumerable<TextSegment> TextSegments
        {
            get => _textSegments;
            private set => SetProperty(ref _textSegments, value);
        }

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
        /// Gets the values of the <see cref="TextFadeEffect"/> enum.
        /// </summary>
        public IEnumerable<EnumDescriptor<TextFadeEffect>> FadeEffectValues { get; } = typeof(TextFadeEffect).GetEnumDescriptors<TextFadeEffect>();

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a track is playing.
        /// </summary>
        /// <remarks>Public so that bindings are set up correctly.</remarks>
        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }

        /// <summary>
        /// Gets the current custom label model.
        /// </summary>
        /// <returns>The custom label model.</returns>
        public CustomLabel GetModel()
        {
            return Model;
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.Color):
                    RefreshSegmentColors();
                    break;
                case nameof(Model.FormatString):
                    ReParseSegments();
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            RefreshSegmentColors();
            ReParseSegments();
        }

        /// <inheritdoc />
        protected override void OnReset()
        {
            base.OnReset();
            RefreshSegmentColors();
            ReParseSegments();
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _source);
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.IsPlaying))
            {
                return;
            }

            OnIsPlayingChanged(_audioSession.IsPlaying);
        }

        private void OnIsPlayingChanged(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private void RefreshSegmentColors()
        {
            foreach (var textSegment in TextSegments)
            {
                textSegment.Color = Color;
            }
        }

        private void ReParseSegments()
        {
            TextSegments = FormattedTextParser.ParseFormattedString(FormatString, Color, _audioSession);
        }
    }
}
