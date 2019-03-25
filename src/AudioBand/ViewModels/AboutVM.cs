using System.Diagnostics;
using System.Reflection;
using AudioBand.Commands;
using AudioBand.Views.Wpf;

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
            OpenProjectLink = new RelayCommand(OpenProjectLinkOnExecute);
            ShowHelp = new RelayCommand<IAboutView>(Execute);
        }

        /// <summary>
        /// Represents the view for the about dialog.
        /// </summary>
        public interface IAboutView
        {
            /// <summary>
            /// Show the about dialong.
            /// </summary>
            void Show();
        }

        /// <summary>
        /// Gets the current audioband version.
        /// </summary>
        public string Version => "AudioBand " + typeof(SettingsWindow).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// Gets the link to the project
        /// </summary>
        public string ProjectLink => @"https://github.com/dsafa/audio-band";

        /// <summary>
        /// Gets the command to show the help dialog.
        /// </summary>
        public RelayCommand<IAboutView> ShowHelp { get; }

        /// <summary>
        /// Gets the command to open the link to the project.
        /// </summary>
        public RelayCommand OpenProjectLink { get; }

        private void Execute(IAboutView helpView)
        {
            helpView?.Show();
        }

        private void OpenProjectLinkOnExecute(object o)
        {
            Process.Start(ProjectLink);
        }
    }
}
