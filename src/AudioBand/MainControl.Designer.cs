using AudioBand.Views.Winforms;

namespace AudioBand
{
    partial class MainControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AlbumArtControl = new AudioBand.Views.Winforms.AlbumArtDisplay();
            this.AlbumArtVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PreviousButtonControl = new AudioBand.Views.Winforms.CustomButton();
            this.PreviousButtonVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PlayPauseButtonControl = new AudioBand.Views.Winforms.CustomButton();
            this.PlayPauseButtonVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.NextButtonControl = new AudioBand.Views.Winforms.CustomButton();
            this.NextButtonVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProgressBarControl = new AudioBand.Views.Winforms.EnhancedProgressBar();
            this.ProgressBarVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AudioBandVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AlbumArtPopup = new AudioBand.Views.Winforms.AlbumArtTooltip();
            this.AlbumArtPopupVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AlbumArtVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousButtonVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPauseButtonVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextButtonVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBarVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioBandVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AlbumArtPopupVMBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AlbumArtControl
            // 
            this.AlbumArtControl.AlbumArt = null;
            this.AlbumArtControl.DataBindings.Add(new System.Windows.Forms.Binding("AlbumArt", this.AlbumArtVMBindingSource, "AlbumArt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalLocation", this.AlbumArtVMBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.AlbumArtVMBindingSource, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtControl.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.AlbumArtVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtControl.Location = new System.Drawing.Point(0, 0);
            this.AlbumArtControl.LogicalLocation = new System.Drawing.Point(0, 0);
            this.AlbumArtControl.LogicalSize = new System.Drawing.Size(0, 0);
            this.AlbumArtControl.Margin = new System.Windows.Forms.Padding(0);
            this.AlbumArtControl.Name = "AlbumArtControl";
            this.AlbumArtControl.Size = new System.Drawing.Size(30, 28);
            this.AlbumArtControl.TabIndex = 0;
            this.AlbumArtControl.TabStop = false;
            this.AlbumArtControl.MouseLeave += new System.EventHandler(this.AlbumArtOnMouseLeave);
            this.AlbumArtControl.MouseHover += new System.EventHandler(this.AlbumArtOnMouseHover);
            // 
            // AlbumArtVMBindingSource
            // 
            this.AlbumArtVMBindingSource.DataSource = typeof(AudioBand.ViewModels.AlbumArtVM);
            // 
            // PreviousButtonControl
            // 
            this.PreviousButtonControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PreviousButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.PreviousButtonVMBindingSource, "Image", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PreviousButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalLocation", this.PreviousButtonVMBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PreviousButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.PreviousButtonVMBindingSource, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PreviousButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.PreviousButtonVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PreviousButtonControl.ForeColor = System.Drawing.Color.White;
            this.PreviousButtonControl.Image = null;
            this.PreviousButtonControl.Location = new System.Drawing.Point(0, 0);
            this.PreviousButtonControl.LogicalLocation = new System.Drawing.Point(0, 0);
            this.PreviousButtonControl.LogicalSize = new System.Drawing.Size(0, 0);
            this.PreviousButtonControl.Margin = new System.Windows.Forms.Padding(0);
            this.PreviousButtonControl.Name = "PreviousButtonControl";
            this.PreviousButtonControl.Size = new System.Drawing.Size(73, 12);
            this.PreviousButtonControl.TabIndex = 0;
            this.PreviousButtonControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PreviousButtonOnClick);
            // 
            // PreviousButtonVMBindingSource
            // 
            this.PreviousButtonVMBindingSource.DataSource = typeof(AudioBand.ViewModels.PreviousButtonVM);
            // 
            // PlayPauseButtonControl
            // 
            this.PlayPauseButtonControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PlayPauseButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.PlayPauseButtonVMBindingSource, "Image", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PlayPauseButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalLocation", this.PlayPauseButtonVMBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PlayPauseButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.PlayPauseButtonVMBindingSource, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PlayPauseButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.PlayPauseButtonVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PlayPauseButtonControl.ForeColor = System.Drawing.Color.White;
            this.PlayPauseButtonControl.Image = null;
            this.PlayPauseButtonControl.Location = new System.Drawing.Point(0, 0);
            this.PlayPauseButtonControl.LogicalLocation = new System.Drawing.Point(0, 0);
            this.PlayPauseButtonControl.LogicalSize = new System.Drawing.Size(0, 0);
            this.PlayPauseButtonControl.Margin = new System.Windows.Forms.Padding(0);
            this.PlayPauseButtonControl.Name = "PlayPauseButtonControl";
            this.PlayPauseButtonControl.Size = new System.Drawing.Size(73, 12);
            this.PlayPauseButtonControl.TabIndex = 1;
            this.PlayPauseButtonControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PlayPauseButtonOnClick);
            // 
            // PlayPauseButtonVMBindingSource
            // 
            this.PlayPauseButtonVMBindingSource.DataSource = typeof(AudioBand.ViewModels.PlayPauseButtonVM);
            // 
            // NextButtonControl
            // 
            this.NextButtonControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.NextButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.NextButtonVMBindingSource, "Image", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NextButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalLocation", this.NextButtonVMBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NextButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.NextButtonVMBindingSource, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NextButtonControl.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.NextButtonVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NextButtonControl.ForeColor = System.Drawing.Color.White;
            this.NextButtonControl.Image = null;
            this.NextButtonControl.Location = new System.Drawing.Point(0, 0);
            this.NextButtonControl.LogicalLocation = new System.Drawing.Point(0, 0);
            this.NextButtonControl.LogicalSize = new System.Drawing.Size(0, 0);
            this.NextButtonControl.Margin = new System.Windows.Forms.Padding(0);
            this.NextButtonControl.Name = "NextButtonControl";
            this.NextButtonControl.Size = new System.Drawing.Size(74, 12);
            this.NextButtonControl.TabIndex = 2;
            this.NextButtonControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NextButtonOnClick);
            // 
            // NextButtonVMBindingSource
            // 
            this.NextButtonVMBindingSource.DataSource = typeof(AudioBand.ViewModels.NextButtonVM);
            // 
            // ProgressBarControl
            // 
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("Progress", this.ProgressBarVMBindingSource, "TrackProgress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("Total", this.ProgressBarVMBindingSource, "TrackLength", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", this.ProgressBarVMBindingSource, "BackgroundColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", this.ProgressBarVMBindingSource, "ForegroundColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.ProgressBarVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalLocation", this.ProgressBarVMBindingSource, "Location", true));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.ProgressBarVMBindingSource, "Size", true));
            this.ProgressBarControl.DataBindings.Add(new System.Windows.Forms.Binding("HoverColor", this.ProgressBarVMBindingSource, "HoverColor", true));
            this.ProgressBarControl.HoverColor = System.Drawing.Color.Empty;
            this.ProgressBarControl.Location = new System.Drawing.Point(0, 0);
            this.ProgressBarControl.LogicalLocation = new System.Drawing.Point(0, 0);
            this.ProgressBarControl.LogicalSize = new System.Drawing.Size(0, 0);
            this.ProgressBarControl.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressBarControl.Name = "ProgressBarControl";
            this.ProgressBarControl.Progress = System.TimeSpan.Parse("00:00:00");
            this.ProgressBarControl.Size = new System.Drawing.Size(250, 2);
            this.ProgressBarControl.TabIndex = 3;
            this.ProgressBarControl.Total = System.TimeSpan.Parse("00:00:00");
            this.ProgressBarControl.Click += new System.EventHandler(this.ProgressBarOnClick);
            // 
            // ProgressBarVMBindingSource
            // 
            this.ProgressBarVMBindingSource.DataSource = typeof(AudioBand.ViewModels.ProgressBarVM);
            // 
            // AudioBandVMBindingSource
            // 
            this.AudioBandVMBindingSource.DataSource = typeof(AudioBand.ViewModels.AudioBandVM);
            // 
            // AlbumArtPopup
            // 
            this.AlbumArtPopup.AlbumArt = null;
            this.AlbumArtPopup.AutomaticDelay = 200;
            this.AlbumArtPopup.DataBindings.Add(new System.Windows.Forms.Binding("Active", this.AlbumArtPopupVMBindingSource, "IsVisible", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtPopup.DataBindings.Add(new System.Windows.Forms.Binding("AlbumArt", this.AlbumArtPopupVMBindingSource, "AlbumArt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtPopup.DataBindings.Add(new System.Windows.Forms.Binding("Margin", this.AlbumArtPopupVMBindingSource, "Margin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtPopup.DataBindings.Add(new System.Windows.Forms.Binding("Size", this.AlbumArtPopupVMBindingSource, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtPopup.DataBindings.Add(new System.Windows.Forms.Binding("XPosition", this.AlbumArtPopupVMBindingSource, "XPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AlbumArtPopup.Margin = 0;
            this.AlbumArtPopup.OwnerDraw = true;
            this.AlbumArtPopup.ShowAlways = true;
            this.AlbumArtPopup.Size = new System.Drawing.Size(0, 0);
            this.AlbumArtPopup.XPosition = 0;
            // 
            // AlbumArtPopupVMBindingSource
            // 
            this.AlbumArtPopupVMBindingSource.DataSource = typeof(AudioBand.ViewModels.AlbumArtPopupVM);
            // 
            // MainControl
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.AlbumArtControl);
            this.Controls.Add(this.PreviousButtonControl);
            this.Controls.Add(this.NextButtonControl);
            this.Controls.Add(this.PlayPauseButtonControl);
            this.Controls.Add(this.ProgressBarControl);
            this.DataBindings.Add(new System.Windows.Forms.Binding("LogicalSize", this.AudioBandVMBindingSource, "Size", true));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainControl";
            ((System.ComponentModel.ISupportInitialize)(this.AlbumArtVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousButtonVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPauseButtonVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextButtonVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBarVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioBandVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AlbumArtPopupVMBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private AlbumArtDisplay AlbumArtControl;
        private CustomButton PreviousButtonControl;
        private CustomButton PlayPauseButtonControl;
        private CustomButton NextButtonControl;
        private EnhancedProgressBar ProgressBarControl;
        private AlbumArtTooltip AlbumArtPopup;
        private System.Windows.Forms.BindingSource AlbumArtVMBindingSource;
        private System.Windows.Forms.BindingSource ProgressBarVMBindingSource;
        private System.Windows.Forms.BindingSource AudioBandVMBindingSource;
        private System.Windows.Forms.BindingSource NextButtonVMBindingSource;
        private System.Windows.Forms.BindingSource PlayPauseButtonVMBindingSource;
        private System.Windows.Forms.BindingSource PreviousButtonVMBindingSource;
        private System.Windows.Forms.BindingSource AlbumArtPopupVMBindingSource;
    }
}
