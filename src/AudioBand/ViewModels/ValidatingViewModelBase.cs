using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// A view model base that extends <see cref="ObservableObject"/> and adds support for validation via <see cref="INotifyDataErrorInfo"/>.
    /// </summary>
    public abstract class ValidatingViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, IEnumerable<string>> _propertyErrors = new Dictionary<string, IEnumerable<string>>();

        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => _propertyErrors.Any(entry => entry.Value?.Any() ?? false);

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors"/>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_propertyErrors.TryGetValue(propertyName, out var errors) && errors.Any())
            {
                return _propertyErrors[propertyName];
            }

            return null;
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="errors">Errors that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(IEnumerable<string> errors, [CallerMemberName] string propertyName = null)
        {
            _propertyErrors.Clear();
            _propertyErrors[propertyName] = errors;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="error">Error that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(string error, [CallerMemberName] string propertyName = null)
        {
            RaiseValidationError(new[] { error }, propertyName);
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">Exception that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(Exception e, [CallerMemberName] string propertyName = null)
        {
            RaiseValidationError(e.ToString(), propertyName);
        }

        /// <summary>
        /// Clears all validation errors.
        /// </summary>
        protected void ClearErrors()
        {
            _propertyErrors.Clear();
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Clears validation errors for a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to clear.</param>
        protected void ClearError([CallerMemberName] string propertyName = null)
        {
            _propertyErrors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
