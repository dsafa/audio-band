namespace AudioBand.ViewModels
{
    /// <summary>
    /// Object that can be reset to a default state
    /// </summary>
    interface IResettableObject
    {
        /// <summary>
        /// Reset to default state
        /// </summary>
        void Reset();
    }
}
