namespace AudioBand.Settings
{
    internal partial class SettingsWindow
    {
        private void InitializeComponent()
        {
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.appearanceTab = new MetroFramework.Controls.MetroTabPage();
            this.appearanceTabLayout = new System.Windows.Forms.TableLayoutPanel();
            this.artistFontDisplay = new MetroFramework.Controls.MetroLabel();
            this.artistColorButton = new MetroFramework.Controls.MetroButton();
            this.artistFontButton = new MetroFramework.Controls.MetroButton();
            this.artistColorDisplay = new System.Windows.Forms.PictureBox();
            this.songFontButton = new MetroFramework.Controls.MetroButton();
            this.songColorButton = new MetroFramework.Controls.MetroButton();
            this.progressColorButton = new MetroFramework.Controls.MetroButton();
            this.songFontDisplay = new MetroFramework.Controls.MetroLabel();
            this.songColorDisplay = new System.Windows.Forms.PictureBox();
            this.progressColorDisplay = new System.Windows.Forms.PictureBox();
            this.artistFontDialog = new System.Windows.Forms.FontDialog();
            this.artistColorDialog = new System.Windows.Forms.ColorDialog();
            this.songFontDialog = new System.Windows.Forms.FontDialog();
            this.songColorDialog = new System.Windows.Forms.ColorDialog();
            this.progressColorDialog = new System.Windows.Forms.ColorDialog();
            this.progressBackColorButton = new MetroFramework.Controls.MetroButton();
            this.progressBackColorDisplay = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.appearanceTab.SuspendLayout();
            this.appearanceTabLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.artistColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBackColorDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.appearanceTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(50, 34);
            this.tabControl.Location = new System.Drawing.Point(20, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(260, 420);
            this.tabControl.TabIndex = 1;
            this.tabControl.UseSelectable = true;
            // 
            // appearanceTab
            // 
            this.appearanceTab.Controls.Add(this.appearanceTabLayout);
            this.appearanceTab.HorizontalScrollbarBarColor = true;
            this.appearanceTab.HorizontalScrollbarHighlightOnWheel = false;
            this.appearanceTab.HorizontalScrollbarSize = 10;
            this.appearanceTab.Location = new System.Drawing.Point(4, 38);
            this.appearanceTab.Name = "appearanceTab";
            this.appearanceTab.Size = new System.Drawing.Size(252, 378);
            this.appearanceTab.TabIndex = 1;
            this.appearanceTab.Text = "Appearance";
            this.appearanceTab.VerticalScrollbarBarColor = true;
            this.appearanceTab.VerticalScrollbarHighlightOnWheel = false;
            this.appearanceTab.VerticalScrollbarSize = 10;
            // 
            // appearanceTabLayout
            // 
            this.appearanceTabLayout.AutoSize = true;
            this.appearanceTabLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.appearanceTabLayout.BackColor = System.Drawing.Color.Transparent;
            this.appearanceTabLayout.ColumnCount = 2;
            this.appearanceTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.appearanceTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.appearanceTabLayout.Controls.Add(this.artistFontDisplay, 1, 0);
            this.appearanceTabLayout.Controls.Add(this.artistColorButton, 0, 1);
            this.appearanceTabLayout.Controls.Add(this.artistFontButton, 0, 0);
            this.appearanceTabLayout.Controls.Add(this.artistColorDisplay, 1, 1);
            this.appearanceTabLayout.Controls.Add(this.songFontButton, 0, 2);
            this.appearanceTabLayout.Controls.Add(this.songColorButton, 0, 3);
            this.appearanceTabLayout.Controls.Add(this.progressColorButton, 0, 4);
            this.appearanceTabLayout.Controls.Add(this.songFontDisplay, 1, 2);
            this.appearanceTabLayout.Controls.Add(this.songColorDisplay, 1, 3);
            this.appearanceTabLayout.Controls.Add(this.progressColorDisplay, 1, 4);
            this.appearanceTabLayout.Controls.Add(this.progressBackColorButton, 0, 5);
            this.appearanceTabLayout.Controls.Add(this.progressBackColorDisplay, 1, 5);
            this.appearanceTabLayout.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.appearanceTabLayout.Location = new System.Drawing.Point(0, 0);
            this.appearanceTabLayout.Margin = new System.Windows.Forms.Padding(0);
            this.appearanceTabLayout.Name = "appearanceTabLayout";
            this.appearanceTabLayout.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.appearanceTabLayout.RowCount = 6;
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.appearanceTabLayout.Size = new System.Drawing.Size(252, 192);
            this.appearanceTabLayout.TabIndex = 2;
            // 
            // artistFontDisplay
            // 
            this.artistFontDisplay.AutoSize = true;
            this.artistFontDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artistFontDisplay.Location = new System.Drawing.Point(129, 9);
            this.artistFontDisplay.Margin = new System.Windows.Forms.Padding(3);
            this.artistFontDisplay.Name = "artistFontDisplay";
            this.artistFontDisplay.Size = new System.Drawing.Size(120, 24);
            this.artistFontDisplay.TabIndex = 1;
            this.artistFontDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // artistColorButton
            // 
            this.artistColorButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artistColorButton.Location = new System.Drawing.Point(0, 39);
            this.artistColorButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.artistColorButton.Name = "artistColorButton";
            this.artistColorButton.Size = new System.Drawing.Size(123, 24);
            this.artistColorButton.TabIndex = 3;
            this.artistColorButton.Text = "Artist Color";
            this.artistColorButton.UseSelectable = true;
            this.artistColorButton.Click += new System.EventHandler(this.ArtistColorButtonOnClick);
            // 
            // artistFontButton
            // 
            this.artistFontButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artistFontButton.Location = new System.Drawing.Point(0, 9);
            this.artistFontButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.artistFontButton.Name = "artistFontButton";
            this.artistFontButton.Size = new System.Drawing.Size(123, 24);
            this.artistFontButton.TabIndex = 4;
            this.artistFontButton.Text = "Artist Font";
            this.artistFontButton.UseSelectable = true;
            this.artistFontButton.Click += new System.EventHandler(this.ArtistFontButtonOnClick);
            // 
            // artistColorDisplay
            // 
            this.artistColorDisplay.BackColor = System.Drawing.Color.Black;
            this.artistColorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artistColorDisplay.Location = new System.Drawing.Point(129, 39);
            this.artistColorDisplay.Name = "artistColorDisplay";
            this.artistColorDisplay.Size = new System.Drawing.Size(120, 24);
            this.artistColorDisplay.TabIndex = 5;
            this.artistColorDisplay.TabStop = false;
            // 
            // songFontButton
            // 
            this.songFontButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songFontButton.Location = new System.Drawing.Point(0, 69);
            this.songFontButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.songFontButton.Name = "songFontButton";
            this.songFontButton.Size = new System.Drawing.Size(123, 24);
            this.songFontButton.TabIndex = 6;
            this.songFontButton.Text = "Song Name Font";
            this.songFontButton.UseSelectable = true;
            this.songFontButton.Click += new System.EventHandler(this.SongFontButtonOnClick);
            // 
            // songColorButton
            // 
            this.songColorButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songColorButton.Location = new System.Drawing.Point(0, 99);
            this.songColorButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.songColorButton.Name = "songColorButton";
            this.songColorButton.Size = new System.Drawing.Size(123, 24);
            this.songColorButton.TabIndex = 7;
            this.songColorButton.Text = "Song Name Color";
            this.songColorButton.UseSelectable = true;
            this.songColorButton.Click += new System.EventHandler(this.SongColorButtonOnClick);
            // 
            // progressColorButton
            // 
            this.progressColorButton.Location = new System.Drawing.Point(0, 129);
            this.progressColorButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.progressColorButton.Name = "progressColorButton";
            this.progressColorButton.Size = new System.Drawing.Size(123, 24);
            this.progressColorButton.TabIndex = 8;
            this.progressColorButton.Text = "Progress Color";
            this.progressColorButton.UseSelectable = true;
            this.progressColorButton.Click += new System.EventHandler(this.ProgressColorButtonOnClick);
            // 
            // songFontDisplay
            // 
            this.songFontDisplay.AutoSize = true;
            this.songFontDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songFontDisplay.Location = new System.Drawing.Point(129, 69);
            this.songFontDisplay.Margin = new System.Windows.Forms.Padding(3);
            this.songFontDisplay.Name = "songFontDisplay";
            this.songFontDisplay.Size = new System.Drawing.Size(120, 24);
            this.songFontDisplay.TabIndex = 9;
            this.songFontDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // songColorDisplay
            // 
            this.songColorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songColorDisplay.Location = new System.Drawing.Point(129, 99);
            this.songColorDisplay.Name = "songColorDisplay";
            this.songColorDisplay.Size = new System.Drawing.Size(120, 24);
            this.songColorDisplay.TabIndex = 11;
            this.songColorDisplay.TabStop = false;
            // 
            // progressColorDisplay
            // 
            this.progressColorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressColorDisplay.Location = new System.Drawing.Point(129, 129);
            this.progressColorDisplay.Name = "progressColorDisplay";
            this.progressColorDisplay.Size = new System.Drawing.Size(120, 24);
            this.progressColorDisplay.TabIndex = 12;
            this.progressColorDisplay.TabStop = false;
            // 
            // progressBackColorButton
            // 
            this.progressBackColorButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBackColorButton.Location = new System.Drawing.Point(0, 159);
            this.progressBackColorButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.progressBackColorButton.Name = "progressBackColorButton";
            this.progressBackColorButton.Size = new System.Drawing.Size(123, 24);
            this.progressBackColorButton.TabIndex = 13;
            this.progressBackColorButton.Text = "Progress Background";
            this.progressBackColorButton.UseSelectable = true;
            // 
            // progressBackColorDisplay
            // 
            this.progressBackColorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBackColorDisplay.Location = new System.Drawing.Point(129, 159);
            this.progressBackColorDisplay.Name = "progressBackColorDisplay";
            this.progressBackColorDisplay.Size = new System.Drawing.Size(120, 24);
            this.progressBackColorDisplay.TabIndex = 0;
            this.progressBackColorDisplay.TabStop = false;
            // 
            // SettingsWindow
            // 
            this.ClientSize = new System.Drawing.Size(300, 500);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "SettingsWindow";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Settings";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.tabControl.ResumeLayout(false);
            this.appearanceTab.ResumeLayout(false);
            this.appearanceTab.PerformLayout();
            this.appearanceTabLayout.ResumeLayout(false);
            this.appearanceTabLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.artistColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBackColorDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage appearanceTab;
        private System.Windows.Forms.FontDialog artistFontDialog;
        private System.Windows.Forms.TableLayoutPanel appearanceTabLayout;
        private MetroFramework.Controls.MetroLabel artistFontDisplay;
        private MetroFramework.Controls.MetroButton artistColorButton;
        private MetroFramework.Controls.MetroButton artistFontButton;
        private System.Windows.Forms.ColorDialog artistColorDialog;
        private System.Windows.Forms.PictureBox artistColorDisplay;
        private MetroFramework.Controls.MetroButton songFontButton;
        private MetroFramework.Controls.MetroButton songColorButton;
        private MetroFramework.Controls.MetroButton progressColorButton;
        private MetroFramework.Controls.MetroLabel songFontDisplay;
        private System.Windows.Forms.PictureBox songColorDisplay;
        private System.Windows.Forms.FontDialog songFontDialog;
        private System.Windows.Forms.ColorDialog songColorDialog;
        private System.Windows.Forms.ColorDialog progressColorDialog;
        private System.Windows.Forms.PictureBox progressColorDisplay;
        private MetroFramework.Controls.MetroButton progressBackColorButton;
        private System.Windows.Forms.PictureBox progressBackColorDisplay;
    }
}
