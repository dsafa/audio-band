using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AudioBand.Views.Wpf.Behaviours
{
    internal class PasswordBehaviour : Behavior<PasswordBox>
    {
        public string Password
        {
            get => (string) GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBehaviour));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += PasswordChanged;
        }


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
