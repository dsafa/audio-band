using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for AboutFlyout.xaml
    /// </summary>
    internal partial class AboutFlyout : AboutVM.IAboutView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutFlyout"/> class.
        /// </summary>
        public AboutFlyout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutFlyout"/> class.
        /// </summary>
        /// <param name="vm">The aboutvm.</param>
        public AboutFlyout(AboutVM vm)
            : this()
        {
            DataContext = VM = vm;
        }

        /// <summary>
        /// Gets the about viewmodel.
        /// </summary>
        public AboutVM VM { get; }

        /// <summary>
        /// Show the flyout.
        /// </summary>
        public void Show()
        {
            IsOpen = true;
        }
    }
}
