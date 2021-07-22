using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using AudioBand.Commands;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for the `about audioband` view.
    /// </summary>
    public class AboutDialogViewModel : ObservableObject
    {
        /// <summary>
        /// Gets the current audioband version.
        /// </summary>
        public string Version => GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// Gets the link to the project.
        /// </summary>
        public string ProjectLink => "https://github.com/svr333/audio-band";

        /// <summary>
        /// Gets the license link.
        /// </summary>
        public string LicenseLink => "https://github.com/svr333/audio-band/blob/master/LICENSE";

        /// <summary>
        /// Gets the third party licenses link.
        /// </summary>
        public string ThirdPartyLicenseLink => "https://github.com/svr333/audio-band/blob/master/LICENSE-3RD-PARTY";

        /// <summary>
        /// Gets the command to open the link to the project.
        /// </summary>
        public ICommand OpenLinkCommand { get; } = new RelayCommand<string>(OpenLinkCommandOnExecute);

        /// <summary>
        /// Gets the command to open the settings folder.
        /// </summary>
        public ICommand OpenSettingsFolderCommand { get; } = new RelayCommand(OpenSettingsFolderCommandOnExecute);

        /// <summary>
        /// Gets the command to open the audioband logs.
        /// </summary>
        public ICommand OpenLog { get; } = new RelayCommand(OpenLogCommandOnExecute);

        private static void OpenLogCommandOnExecute()
        {
            Process.Start(Path.Combine(Path.GetTempPath(), "AudioBand.log"));
        }

        private static void OpenSettingsFolderCommandOnExecute()
        {
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand"));
        }

        private static void OpenLinkCommandOnExecute(string link)
        {
            Process.Start(link);
        }
    }
}
