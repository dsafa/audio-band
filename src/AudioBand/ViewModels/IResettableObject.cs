namespace AudioBand.ViewModels
{
    /// <summary>
    /// Provides functionality to reset back to the initial state.
    /// </summary>
    public interface IResettableObject
    {
        /// <summary>
        /// Reset to the initial state.
        /// </summary>
        void Reset();
    }
}
