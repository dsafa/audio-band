using AudioBand.AudioSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A placeholder text that has values based on the current song.
    /// </summary>
    public abstract class TextPlaceholder
    {
        private readonly HashSet<string> _propertyFilter = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The parameters passed to the text format.</param>
        /// <param name="audioSession">The audio session to use for the placeholder value.</param>
        protected TextPlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession audioSession)
        {
            Session = audioSession;
            Session.PropertyChanged += AudioSessionOnPropertyChanged;

            // TODO parameters
        }

        /// <summary>
        /// Occurs when the placeholders text has changed.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Gets the audio session.
        /// </summary>
        protected IAudioSession Session { get; private set; }

        /// <summary>
        /// Gets the current text value for the placeholder.
        /// </summary>
        /// <returns>The value.</returns>
        public abstract string GetText();

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event.
        /// </summary>
        protected void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the parameter from the name.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The value of the parameter or null if not passed in.</returns>
        protected string GetParameter(string parameterName)
        {
            return null;
        }

        /// <summary>
        /// Adds a filter for <see cref="OnAudioSessionPropertyChanged"/>.
        /// </summary>
        /// <param name="audioSessionPropertyName">The property name to filter.</param>
        protected void AddSessionPropertyFilter(string audioSessionPropertyName)
        {
            _propertyFilter.Add(audioSessionPropertyName);
        }

        /// <summary>
        /// Called when the audio session property value changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnAudioSessionPropertyChanged(string propertyName)
        {
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyFilter.Contains(e.PropertyName) || _propertyFilter.Count == 0)
            {
                OnAudioSessionPropertyChanged(e.PropertyName);
            }
        }
    }
}
