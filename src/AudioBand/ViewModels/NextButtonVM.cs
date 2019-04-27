using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the next button.
    /// </summary>
    public class NextButtonVM : PlaybackButtonVMBase<NextButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NextButtonVM"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        /// <param name="dialogService">The dialog service.</param>
        public NextButtonVM(IAppSettings appSettings, IResourceLoader resourceLoader, IDialogService dialogService)
            : base(appSettings, resourceLoader, dialogService, appSettings.NextButton)
        {
        }

        /// <inheritdoc />
        protected override NextButton GetReplacementModel()
        {
            return AppSettings.NextButton;
        }

        /// <inheritdoc />
        protected override IImage GetDefaultImage()
        {
            return ResourceLoader.TryLoadImageFromPath(ImagePath, ResourceLoader.DefaultNextImage);
        }
    }
}
