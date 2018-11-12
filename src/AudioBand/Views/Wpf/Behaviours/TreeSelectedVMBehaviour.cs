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
        /// The current selected view model in the tree.
        /// </summary>
        public object SelectedVM
        {
            get => GetValue(SelectedVMProperty);
            set => SetValue(SelectedVMProperty, value);
        }

        public static readonly DependencyProperty SelectedVMProperty = DependencyProperty.Register(nameof(SelectedVM), typeof(object), typeof(TreeSelectedVMBehaviour));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

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
            else if (e.NewValue is ViewModelBase vm)
            {
                SelectedVM = vm;
            }
        }
    }
}
