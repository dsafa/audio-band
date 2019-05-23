using System.ComponentModel;
using System.Windows.Media;

namespace AudioBand.Resources.Theming
{
    /// <summary>
    /// Provides theme resources.
    /// </summary>
    public interface IThemeDictionary : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the theme brush.
        /// </summary>
        /// <param name="key">Theme resource key.</param>
        /// <returns>The theme brush,</returns>
        Brush this[string key] { get; }
    }
}
