using System.Windows.Input;

namespace AudioBand.Settings
{
    public static class SettingsCommands
    {
        public static readonly RoutedCommand ResetImagePath = new RoutedCommand(nameof(ResetImagePath), typeof(SettingsCommands));
        public static readonly RoutedCommand ChooseImage = new RoutedCommand(nameof(ChooseImage), typeof(SettingsCommands));
        public static readonly RoutedCommand AddLabel = new RoutedCommand(nameof(AddLabel), typeof(SettingsCommands));
        public static readonly RoutedCommand DeleteLabel = new RoutedCommand(nameof(DeleteLabel), typeof(SettingsCommands));
        public static readonly RoutedCommand ShowAbout = new RoutedCommand(nameof(ShowAbout), typeof(SettingsCommands));
    }
}
