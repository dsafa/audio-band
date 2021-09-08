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
        public const string DefaultProfileName = "Default";

        /// <summary>
        /// The idle profile name.
        /// </summary>
        public const string DefaultIdleProfileName = "Idle";

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
        /// Creates an idle profile used for when the player is idle. 
        /// </summary>
        /// <returns>A default idle profile.</returns>
        public static UserProfile CreateDefaultIdleProfile()
        {
            return new UserProfile()
            {
                Name = DefaultIdleProfileName,
                GeneralSettings = new GeneralSettings()
                {
                    Width = 40,
                    Height = 30
                },
                AlbumArt = new AlbumArt()
                {
                    XPosition = 0,
                    YPosition = 0
                },
                AlbumArtPopup = new AlbumArtPopup()
                {
                    IsVisible = false
                },
                PlayPauseButton = new PlayPauseButton()
                {
                    IsVisible = false
                },
                NextButton = new NextButton()
                {
                    IsVisible = false
                },
                PreviousButton = new PreviousButton()
                {
                    IsVisible = false
                },
                RepeatModeButton = new RepeatModeButton()
                {
                    IsVisible = false
                },
                ShuffleModeButton = new ShuffleModeButton()
                {
                    IsVisible = false
                },
                ProgressBar = new ProgressBar()
                {
                    IsVisible = false
                },
                CustomLabels = new List<CustomLabel>()
            };
        }

        /// <summary>
        /// Creates an array of UserProfiles to fill up the default settings.
        /// </summary>
        /// <returns>The array of default profiles</returns>
        public static UserProfile[] CreateDefaultProfiles()
        {
            var profiles = new UserProfile[5];
            profiles[0] = CreateDefaultIdleProfile();
            profiles[1] = CreateDefaultProfile(DefaultProfileName);
            profiles[2] = new UserProfile()
            {
                Name = "Default (Compact)",
                GeneralSettings = new GeneralSettings()
                {
                    Width = 245.0,
                    Height = 30.0
                },
                AlbumArt = new AlbumArt()
                {
                    PlaceholderPath = "",
                    IsVisible = true,
                    Width = 30.0,
                    Height = 30.0,
                    XPosition = 90.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft
                },
                AlbumArtPopup = new AlbumArtPopup()
                {
                    IsVisible = true,
                    Width = 250.0,
                    Height = 250.0,
                    XPosition = -110.0,
                    Margin = 4.0
                },
                PlayPauseButton = new PlayPauseButton()
                {
                    IsVisible = true,
                    Width = 30.0,
                    Height = 13.0,
                    XPosition = 170.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft,
                    PlayContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    },
                    PauseContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = "",
                    }
                },
                NextButton = new NextButton()
                {
                    IsVisible = true,
                    Width = 40.0,
                    Height = 13.0,
                    XPosition = 190.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft,
                    Content = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                PreviousButton = new PreviousButton()
                {
                    IsVisible = true,
                    Width = 30.0,
                    Height = 13.0,
                    XPosition = 145.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft,
                    Content = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                RepeatModeButton = new RepeatModeButton()
                {
                    IsVisible = true,
                    Width = 40.0,
                    Height = 13.0,
                    XPosition = 215.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft,
                    RepeatOffContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    },
                    RepeatContextContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    },
                    RepeatTrackContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                ShuffleModeButton = new ShuffleModeButton()
                {
                    IsVisible = true,
                    Width = 40.0,
                    Height = 13.0,
                    XPosition = 117.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft,
                    ShuffleOffContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    },
                    ShuffleOnContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                ProgressBar = new ProgressBar()
                {
                    IsVisible = true,
                    Width = 60.0,
                    Height = 4.0,
                    XPosition = 152.0,
                    YPosition = 22.0,
                    Anchor = PositionAnchor.TopLeft
                },
                CustomLabels = new List<CustomLabel>()
                {
                    new CustomLabel
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 12.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{length}",
                        Alignment = CustomLabel.TextAlignment.Right,
                        Name = "Song Length",
                        ScrollSpeed = 5000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 25.0,
                        Height = 15.0,
                        XPosition = 217.0,
                        YPosition = 14.0,
                        Anchor = PositionAnchor.TopLeft
                    },
                    new CustomLabel
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 12.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{time}",
                        Alignment = CustomLabel.TextAlignment.Left,
                        Name = "Song Progress",
                        ScrollSpeed = 5000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 30.0,
                        Height = 15.0,
                        XPosition = 125.0,
                        YPosition = 14.0,
                        Anchor = PositionAnchor.TopLeft
                    },
                    new CustomLabel
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 14.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{song}",
                        Alignment = CustomLabel.TextAlignment.Right,
                        Name = "Song Name",
                        ScrollSpeed = 4000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 90.0,
                        Height = 20.0,
                        XPosition = -6.0,
                        YPosition = -2.0,
                        Anchor = PositionAnchor.TopLeft
                    },
                    new CustomLabel
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 12.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{artist}",
                        Alignment = CustomLabel.TextAlignment.Right,
                        Name = "Artist",
                        ScrollSpeed = 4000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 90.0,
                        Height = 20.0,
                        XPosition = -6.0,
                        YPosition = 13.0,
                        Anchor = PositionAnchor.TopLeft,
                    }
                }
            };

            profiles[3] = new UserProfile()
            {
                Name = "Compact",
                GeneralSettings = new GeneralSettings()
                {
                    Width = 210.0,
                    Height = 30.0
                },
                AlbumArt = new AlbumArt()
                {
                    PlaceholderPath = "",
                    IsVisible = true,
                    Width = 25.0,
                    Height = 25.0,
                    XPosition = 5.0,
                    YPosition = -1.5,
                    Anchor = PositionAnchor.TopLeft
                },
                AlbumArtPopup = new AlbumArtPopup()
                {
                    IsVisible = true,
                    Width = 250.0,
                    Height = 250.0,
                    XPosition = -110.0,
                    Margin = 4.0
                },
                PlayPauseButton = new PlayPauseButton()
                {
                    IsVisible = true,
                    Width = 24.0,
                    Height = 12.0,
                    XPosition = 90.0,
                    YPosition = 13.5,
                    Anchor = PositionAnchor.TopLeft,
                    PlayContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    },
                    PauseContent = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                NextButton = new NextButton()
                {
                    IsVisible = true,
                    Width = 24.0,
                    Height = 12.0,
                    XPosition = 135.0,
                    YPosition = 13.5,
                    Anchor = PositionAnchor.TopLeft,
                    Content = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                PreviousButton = new PreviousButton()
                {
                    IsVisible = true,
                    Width = 30.0,
                    Height = 12.0,
                    XPosition = 45.0,
                    YPosition = 13.5,
                    Anchor = PositionAnchor.TopLeft,
                    Content = new ButtonContent()
                    {
                        ContentType = ButtonContentType.Text,
                        FontFamily = "Segoe MDL2 Assets",
                        Text = ""
                    }
                },
                RepeatModeButton = new RepeatModeButton()
                {
                    IsVisible = false
                },
                ShuffleModeButton = new ShuffleModeButton()
                {
                    IsVisible = false
                },
                ProgressBar = new ProgressBar()
                {
                    IsVisible = true,
                    Width = 200.0,
                    Height = 2.5,
                    XPosition = 0.0,
                    YPosition = 26.0,
                    Anchor = PositionAnchor.TopLeft
                },
                CustomLabels = new List<CustomLabel>
                {
                    new CustomLabel()
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 12.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{artist} - {song}",
                        Alignment = CustomLabel.TextAlignment.Center,
                        Name = "Artist - Name",
                        ScrollSpeed = 4000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 163.0,
                        Height = 20.0,
                        XPosition = 35.0,
                        YPosition = -2.0,
                        Anchor = PositionAnchor.TopLeft
                    }
                }
            };

            profiles[4] = new UserProfile()
            {
                Name = "No Controls",
                GeneralSettings = new GeneralSettings()
                {
                    Width = 235.0,
                    Height = 30.0
                },
                AlbumArt = new AlbumArt()
                {
                    PlaceholderPath = "",
                    IsVisible = true,
                    Width = 30.0,
                    Height = 30.0,
                    XPosition = 205.0,
                    YPosition = 0.0,
                    Anchor = PositionAnchor.TopLeft
                },
                AlbumArtPopup = new AlbumArtPopup()
                {
                    IsVisible = true,
                    Width = 250.0,
                    Height = 250.0,
                    XPosition = -110.0,
                    Margin = 4.0,
                },
                PlayPauseButton = new PlayPauseButton()
                {
                    IsVisible = false
                },
                NextButton = new NextButton()
                {
                    IsVisible = false
                },
                PreviousButton = new PreviousButton()
                {
                    IsVisible = false
                },
                RepeatModeButton = new RepeatModeButton()
                {
                    IsVisible = false
                },
                ShuffleModeButton = new ShuffleModeButton()
                {
                    IsVisible = false
                },
                ProgressBar = new ProgressBar()
                {
                    IsVisible = false
                },
                CustomLabels = new List<CustomLabel>
                {
                    new CustomLabel()
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 14.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{song}",
                        Alignment = CustomLabel.TextAlignment.Right,
                        Name = "Song Name",
                        ScrollSpeed = 5000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 200.0,
                        Height = 20.0,
                        XPosition = 0.0,
                        YPosition = -2.0
                    },
                    new CustomLabel()
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 12.0f,
                        Color = Color.FromRgb(195, 195, 195),
                        FormatString = "{artist}",
                        Alignment = CustomLabel.TextAlignment.Right,
                        Name = "Artist",
                        ScrollSpeed = 5000,
                        TextOverflow = TextOverflow.Scroll,
                        ScrollBehavior = ScrollBehavior.Always,
                        FadeEffect = TextFadeEffect.OnlyWhenScrolling,
                        LeftFadeOffset = 0.1,
                        RightFadeOffset = 0.9,
                        IsVisible = true,
                        Width = 200.0,
                        Height = 20.0,
                        XPosition = 0.0,
                        YPosition = 13.0,
                        Anchor = PositionAnchor.TopLeft
                    }
                }
            };

            return profiles;
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
