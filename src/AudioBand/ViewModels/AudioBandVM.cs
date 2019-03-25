using System.Drawing;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the general application.
    /// </summary>
    public class AudioBandVM : ViewModelBase<Models.AudioBand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandVM"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        public AudioBandVM(IAppSettings appsettings)
            : base(appsettings.AudioBand) { }

        [PropertyChangeBinding(nameof(Models.AudioBand.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(Models.AudioBand.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets the size of the toolbar.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);
    }
}
