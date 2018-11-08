using AudioBand.Commands;
using AudioBand.Views.Wpf;
using System.Diagnostics;
using System.Reflection;

namespace AudioBand.ViewModels
{
    internal class HelpVM
    {
        public string Version { get; } = "AudioBand " + typeof(SettingsWindow).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public string ProjectLink { get; } = @"https://github.com/dsafa/audio-band";

        public RelayCommand<IHelpView> ShowHelp { get; }
        public RelayCommand OpenProjectLink { get; }

        public HelpVM()
        {
            OpenProjectLink = new RelayCommand(OpenProjectLinkOnExecute);
            ShowHelp = new RelayCommand<IHelpView>(Execute);
        }

        private void Execute(IHelpView helpView)
        {
            helpView.Show();
        }

        private void OpenProjectLinkOnExecute(object o)
        {
            Process.Start(ProjectLink);
        }
    }

    internal interface IHelpView
    {
        void Show();
    }
}
