using System.Diagnostics;
using System.Reflection;
using AudioBand.Commands;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the `about audioband` view.
    /// </summary>
    public class AboutVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutVM"/> class.
        /// </summary>
        public AboutVM()
        {
            OpenLinkCommand = new RelayCommand<string>(OpenLinkCommandOnExecute);
        }

        /// <summary>
        /// Gets the current audioband version.
        /// </summary>
        public string Version => GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// Gets the link to the project.
        /// </summary>
        public string ProjectLink => "https://github.com/dsafa/audio-band";

        /// <summary>
        /// Gets the license link.
        /// </summary>
        public string LicenseLink => "https://github.com/dsafa/audio-band/blob/master/LICENSE";

        /// <summary>
        /// Gets the third party licenses link.
        /// </summary>
        public string ThirdPartyLicenseLink => "https://github.com/dsafa/audio-band/blob/master/LICENSE-3RD-PARTY";

        /// <summary>
        /// Gets the command to open the link to the project.
        /// </summary>
        public RelayCommand<string> OpenLinkCommand { get; }

        private void OpenLinkCommandOnExecute(string link)
        {
            Process.Start(link);
        }
    }
}
