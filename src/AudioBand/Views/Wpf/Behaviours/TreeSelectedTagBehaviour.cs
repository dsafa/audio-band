using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AudioBand.Views.Wpf.Behaviours
{
    /// <summary>
    /// Behaviour to bind the selected item's tag
    /// </summary>
    internal class TreeSelectedTagBehaviour : Behavior<TreeView>
    {
        public object SelectedTag
        {
            get => GetValue(SelectedTagProperty);
            set => SetValue(SelectedTagProperty, value);
        }

        public static readonly DependencyProperty SelectedTagProperty =
            DependencyProperty.Register("SelectedTag", typeof(object), typeof(TreeSelectedTagBehaviour), new UIPropertyMetadata(null, OnSelectedTagChanged));

        private static void OnSelectedTagChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                item.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
        }

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
                SelectedTag = item.Tag;
            }
        }
    }
}
