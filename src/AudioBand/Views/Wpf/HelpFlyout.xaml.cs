using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for AboutFlyout.xaml
    /// </summary>
    internal partial class HelpFlyout : IHelpView
    {
        public HelpVM VM { get; }

        public HelpFlyout()
        {
            InitializeComponent();
        }

        public HelpFlyout(HelpVM vm) : this()
        {
            DataContext = VM = vm;
        }

        public void Show()
        {
            IsOpen = true;
        }
    }
}
