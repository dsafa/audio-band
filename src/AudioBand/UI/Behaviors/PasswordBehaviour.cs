using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AudioBand.UI
{
    /// <summary>
    /// Behaviour for a password box to allow binding. From http://www.wpftutorial.net/PasswordBox.html.
    /// </summary>
    internal class PasswordBehaviour : Behavior<PasswordBox>
    {
        /// <summary>
        /// Dependency property for the password.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty
            = DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBehaviour), new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Dependency property for <see cref="IsUpdating"/>.
        /// </summary>
        public static readonly DependencyProperty IsUpdatingProperty
            = DependencyProperty.RegisterAttached(nameof(IsUpdating), typeof(bool), typeof(PasswordBehaviour), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets the current password.
        /// </summary>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the password is being updated.
        /// </summary>
        public bool IsUpdating
        {
            get => (bool)GetValue(IsUpdatingProperty);
            set => SetValue(IsUpdatingProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Password = Password;
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

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behaviour = (PasswordBehaviour)sender;
            behaviour.PasswordPropertyChanged((string)e.NewValue);
        }

        private void PasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            IsUpdating = true;
            Password = AssociatedObject.Password;
            IsUpdating = false;
        }

        private void PasswordPropertyChanged(string value)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            AssociatedObject.PasswordChanged -= PasswordChanged;

            if (!IsUpdating)
            {
                AssociatedObject.Password = value;
            }

            AssociatedObject.PasswordChanged += PasswordChanged;
        }
    }
}
