using AudioBand.Models;

namespace AudioBand.Settings.Models
{
    internal interface ISettings
    {
        string Version { get; }

        string AudioSource { get; set; }

        T GetModel<T>();
    }
}
