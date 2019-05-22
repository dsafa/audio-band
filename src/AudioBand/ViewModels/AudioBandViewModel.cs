using System;
using System.Diagnostics;
using AudioBand.Settings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the general application.
    /// </summary>
    public class AudioBandViewModel : ViewModelBase<Models.AudioBand>
    {
        private readonly IAppSettings _appsettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandViewModel"/> class.
        /// </summary>
        /// <param name="appsettings">The app settings.</param>
        public AudioBandViewModel(IAppSettings appsettings)
            : base(appsettings.AudioBand)
        {
            _appsettings = appsettings;
            appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        [PropertyChangeBinding(nameof(Models.AudioBand.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(Models.AudioBand.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.AudioBand);
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
