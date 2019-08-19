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
        /// The default profile name.
        /// </summary>
        public const string DefaultProfileName = "Default Profile";

        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the saved album art popup model.
        /// </summary>
        public AlbumArtPopup AlbumArtPopup { get; set; }

        /// <summary>
        /// Gets or sets the saved album art model.
        /// </summary>
        public AlbumArt AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the saved general settings model.
        /// </summary>
        public GeneralSettings GeneralSettings { get; set; }

        /// <summary>
        /// Gets or sets the saved labels.
        /// </summary>
        public List<CustomLabel> CustomLabels { get; set; }

        /// <summary>
        /// Gets or sets the saved button model.
        /// </summary>
        public NextButton NextButton { get; set; }

        /// <summary>
        /// Gets or sets the saved previous button model.
        /// </summary>
        public PreviousButton PreviousButton { get; set; }

        /// <summary>
        /// Gets or sets the saved play pause button model.
        /// </summary>
        public PlayPauseButton PlayPauseButton { get; set; }

        /// <summary>
        /// Gets or sets the repeat mode button model.
        /// </summary>
        public RepeatModeButton RepeatModeButton { get; set; }

        /// <summary>
        /// Gets or sets the shuffle mode button model.
        /// </summary>
        public ShuffleModeButton ShuffleModeButton { get; set; }

        /// <summary>
        /// Gets or sets the saved progress bar model.
        /// </summary>
        public ProgressBar ProgressBar { get; set; }

        /// <summary>
        /// Creates an initial profile with default values.
        /// </summary>
        /// <param name="name">The name of the new profile.</param>
        /// <returns>A new profile with default values.</returns>
        public static UserProfile CreateDefaultProfile(string name)
        {
            return new UserProfile()
            {
                Name = name,
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

        /// <summary>
        /// Gets a unique profile name from a target name.
        /// </summary>
        /// <param name="profileNames">Collection of current profile names.</param>
        /// <param name="name">Target name.</param>
        /// <returns>The target name if available, otherwise the target name appended with a number.</returns>
        public static string GetUniqueProfileName(ICollection<string> profileNames, string name)
        {
            string newName = name;
            int count = 0;
            while (profileNames.Contains(newName))
            {
                newName += count;
            }

            return newName;
        }
    }
}
