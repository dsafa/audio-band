using AudioBand.ViewModels;

namespace AudioBand.Messages
{
    /// <summary>
    /// Global edit messages for all <see cref="ViewModelBase"/>
    /// </summary>
    public enum EditMessage
    {
        /// <summary>
        /// Accept edits
        /// </summary>
        AcceptEdits,

        /// <summary>
        /// Cancel current edits
        /// </summary>
        CancelEdits
    }
}
