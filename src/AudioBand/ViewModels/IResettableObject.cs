namespace AudioBand.ViewModels
{
    /// <summary>
    /// Provides functionality to reset back to the initial state.
    /// </summary>
    internal interface IResettableObject
    {
        /// <summary>
        /// Reset to the initial state.
        /// </summary>
        void Reset();
    }
}
