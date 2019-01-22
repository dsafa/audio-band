using AudioBand.AudioSource;
using System.Runtime.Serialization;

namespace AudioBand.ServiceContracts
{
    /// <summary>
    /// Data contract for <see cref="SettingChangedEventArgs"/>.
    /// </summary>
    [DataContract]
    public class SettingChangedInfo
    {
        /// <summary>
        /// Data member for <see cref="SettingChangedEventArgs.PropertyName"/>.
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        public static explicit operator SettingChangedInfo(SettingChangedEventArgs e)
        {
            return new SettingChangedInfo { PropertyName = e.PropertyName };
        }

        public static explicit operator SettingChangedEventArgs(SettingChangedInfo info)
        {
            return new SettingChangedEventArgs(info.PropertyName);
        }
    }
}
