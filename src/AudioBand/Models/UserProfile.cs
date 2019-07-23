using System.Collections.Generic;
using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Profile that contains the user's saved settings.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the saved album art popup model.
        /// </summary>
        public AlbumArtPopup AlbumArtPopup { get; private set; }

        /// <summary>
        /// Gets the saved album art model.
        /// </summary>
        public AlbumArt AlbumArt { get; private set; }

        /// <summary>
        /// Gets the saved general settings model.
        /// </summary>
        public GeneralSettings GeneralSettings { get; private set; }

        /// <summary>
        /// Gets the saved labels.
        /// </summary>
        public List<CustomLabel> CustomLabels { get; private set; }

        /// <summary>
        /// Gets the saved button model.
        /// </summary>
        public NextButton NextButton { get; private set; }

        /// <summary>
        /// Gets the saved previous button model.
        /// </summary>
        public PreviousButton PreviousButton { get; private set; }

        /// <summary>
        /// Gets the saved play pause button model.
        /// </summary>
        public PlayPauseButton PlayPauseButton { get; private set; }

        /// <summary>
        /// Gets the repeat mode button model.
        /// </summary>
        public RepeatModeButton RepeatModeButton { get; private set; }

        /// <summary>
        /// Gets the shuffle mode button model.
        /// </summary>
        public ShuffleModeButton ShuffleModeButton { get; private set; }

        /// <summary>
        /// Gets the saved progress bar model.
        /// </summary>
        public ProgressBar ProgressBar { get; private set; }

        /// <summary>
        /// Creates an initial profile with default values.
        /// </summary>
        /// <returns>A new profile with default values.</returns>
        public static UserProfile CreateInitialProfile()
        {
            return new UserProfile()
            {
                GeneralSettings = new GeneralSettings(),
                AlbumArt = new AlbumArt(),
                AlbumArtPopup = new AlbumArtPopup(),
                PlayPauseButton = new PlayPauseButton(),
                NextButton = new NextButton(),
                PreviousButton = new PreviousButton(),
                RepeatModeButton = new RepeatModeButton(),
                ShuffleModeButton = new ShuffleModeButton(),
                ProgressBar = new ProgressBar(),
                CustomLabels = new List<CustomLabel>
                {
                    new CustomLabel
                    {
                        Name = "Song Length",
                        Width = 40,
                        Height = 15,
                        FontSize = 12,
                        XPosition = 460,
                        YPosition = 14,
                        FormatString = "{length}",
                        Color = Color.FromRgb(195, 195, 195),
                        Alignment = CustomLabel.TextAlignment.Right,
                    },
                    new CustomLabel
                    {
                        Name = "Song Progress",
                        Width = 40,
                        Height = 15,
                        FontSize = 12,
                        XPosition = 280,
                        YPosition = 14,
                        FormatString = "{time}",
                        Color = Color.FromRgb(195, 195, 195),
                        Alignment = CustomLabel.TextAlignment.Left,
                    },
                    new CustomLabel
                    {
                        Name = "Song Name",
                        Width = 240,
                        Height = 20,
                        XPosition = 0,
                        YPosition = -2,
                        FontSize = 14,
                        FormatString = "{song}",
                        Color = Colors.White,
                        Alignment = CustomLabel.TextAlignment.Right,
                    },
                    new CustomLabel
                    {
                        Name = "Artist",
                        Width = 240,
                        Height = 20,
                        XPosition = 0,
                        YPosition = 13,
                        FontSize = 12,
                        FormatString = "{artist}",
                        Color = Color.FromRgb(170, 170, 170),
                        Alignment = CustomLabel.TextAlignment.Right,
                    },
                },
            };
        }
    }
}
