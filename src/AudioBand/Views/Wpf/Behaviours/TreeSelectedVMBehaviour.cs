using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf.Behaviours
{
    /// <summary>
    /// Behaviour to expose a binding to a <see cref="ViewModelBase"/> if selected in a tree.
    /// </summary>
    internal class TreeSelectedVMBehaviour : Behavior<TreeView>
    {
        /// <summary>
        /// Dependency property for <see cref="SelectedVM"/>.
        /// </summary>
        public static readonly DependencyProperty SelectedVMProperty = DependencyProperty.Register(nameof(SelectedVM), typeof(object), typeof(TreeSelectedVMBehaviour));

        /// <summary>
        /// Gets or sets the current selected view model in the tree.
        /// </summary>
        public object SelectedVM
        {
            get => GetValue(SelectedVMProperty);
            set => SetValue(SelectedVMProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                SelectedVM = item.Tag;
            }
            else
            {
                SelectedVM = e.NewValue;
            }
        }
    }
}
