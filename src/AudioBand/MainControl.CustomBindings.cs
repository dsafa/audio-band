using AudioBand.ViewModels;
using System.ComponentModel;
using System.Windows.Forms;

namespace AudioBand
{
    partial class MainControl
    {
        private BindingSource AlbumArtPopupVMBindingSource;

        private void CustomInitializeComponent()
        {
            AlbumArtPopupVMBindingSource = new BindingSource(components);
            ((ISupportInitialize)AlbumArtPopupVMBindingSource).BeginInit();

            DataBindings.Add(new Binding(nameof(AlbumArtPopupWidth), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.Width), true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding(nameof(AlbumArtPopupHeight), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.Height), true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding(nameof(AlbumArtPopupX), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.XPosition), true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding(nameof(AlbumArtPopupY), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.Margin), true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding(nameof(AlbumArtPopupIsVisible), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.IsVisible), true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding(nameof(AlbumArtPopupImage), AlbumArtPopupVMBindingSource, nameof(AlbumArtPopupVM.AlbumArt), true, DataSourceUpdateMode.OnPropertyChanged));

            AlbumArtPopupVMBindingSource.DataSource = typeof(AlbumArtPopupVM);

            ((ISupportInitialize)AlbumArtPopupVMBindingSource).EndInit();
        }
    }
}
