using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Behaviour for a password box.
    /// </summary>
    internal class PasswordBehaviour : Behavior<PasswordBox>
    {
        /// <summary>
        /// Dependency property for the password.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBehaviour));

        /// <summary>
        /// Gets or sets the current password.
        /// </summary>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += PasswordChanged;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged -= PasswordChanged;
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            Password = AssociatedObject.Password;
        }
    }
}
