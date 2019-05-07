using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the previous button.
    /// </summary>
    public class PreviousButtonVM : PlaybackButtonVMBase<PreviousButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButtonVM"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        /// <param name="dialogService">The dialog service.</param>
        public PreviousButtonVM(IAppSettings appSettings, IResourceLoader resourceLoader, IDialogService dialogService)
            : base(appSettings, resourceLoader, dialogService, appSettings.PreviousButton)
        {
        }

        /// <inheritdoc />
        protected override PreviousButton GetReplacementModel()
        {
            return AppSettings.PreviousButton;
        }

        /// <inheritdoc />
        protected override IImage GetDefaultDrawingImage()
        {
            return ResourceLoader.DefaultPreviousImage;
        }
    }
}
