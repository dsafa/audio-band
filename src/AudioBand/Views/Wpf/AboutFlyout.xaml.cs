using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for AboutFlyout.xaml
    /// </summary>
    internal partial class AboutFlyout : AboutVM.IAboutView
    {
        public AboutVM VM { get; }

        public AboutFlyout()
        {
            InitializeComponent();
        }

        public AboutFlyout(AboutVM vm) : this()
        {
            DataContext = VM = vm;
        }

        public void Show()
        {
            IsOpen = true;
        }
    }
}
