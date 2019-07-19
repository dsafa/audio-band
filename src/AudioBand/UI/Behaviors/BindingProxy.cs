using System.Windows;

namespace AudioBand.UI
{
    /// <summary>
    /// Binding proxy.
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// Dependency property for <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the proxied value.
        /// </summary>
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
