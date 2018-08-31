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
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.albumArt = new System.Windows.Forms.PictureBox();
            this.nowPlayingText = new AudioBand.NowPlayingDisplay();
            this.buttonsTable = new System.Windows.Forms.TableLayoutPanel();
            this.previousButton = new System.Windows.Forms.Button();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.audioProgress = new AudioBand.EnhancedProgressBar();
            this.mainTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).BeginInit();
            this.buttonsTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTable.Controls.Add(this.albumArt, 0, 0);
            this.mainTable.Controls.Add(this.nowPlayingText, 1, 0);
            this.mainTable.Controls.Add(this.buttonsTable, 1, 1);
            this.mainTable.Controls.Add(this.audioProgress, 0, 2);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Margin = new System.Windows.Forms.Padding(0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 3;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.mainTable.Size = new System.Drawing.Size(250, 30);
            this.mainTable.TabIndex = 0;
            // 
            // albumArt
            // 
            this.albumArt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.albumArt.Location = new System.Drawing.Point(0, 0);
            this.albumArt.Margin = new System.Windows.Forms.Padding(0);
            this.albumArt.Name = "albumArt";
            this.mainTable.SetRowSpan(this.albumArt, 2);
            this.albumArt.Size = new System.Drawing.Size(30, 28);
            this.albumArt.TabIndex = 0;
            this.albumArt.TabStop = false;
            this.albumArt.MouseLeave += new System.EventHandler(this.AlbumArtOnMouseLeave);
            this.albumArt.MouseHover += new System.EventHandler(this.AlbumArtOnMouseHover);
            // 
            // nowPlayingText
            // 
            this.nowPlayingText.ArtistColor = System.Drawing.Color.Empty;
            this.nowPlayingText.ArtistFont = null;
            this.nowPlayingText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nowPlayingText.Location = new System.Drawing.Point(30, 0);
            this.nowPlayingText.Margin = new System.Windows.Forms.Padding(0);
            this.nowPlayingText.Name = "nowPlayingText";
            this.nowPlayingText.Size = new System.Drawing.Size(220, 14);
            this.nowPlayingText.TabIndex = 2;
            this.nowPlayingText.Text = "label1";
            this.nowPlayingText.TrackNameColor = System.Drawing.Color.White;
            this.nowPlayingText.TrackNameFont = new System.Drawing.Font("Segoe UI", 8.5F);
            // 
            // buttonsTable
            // 
            this.buttonsTable.ColumnCount = 3;
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.Controls.Add(this.previousButton, 0, 0);
            this.buttonsTable.Controls.Add(this.playPauseButton, 1, 0);
            this.buttonsTable.Controls.Add(this.nextButton, 2, 0);
            this.buttonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsTable.Location = new System.Drawing.Point(30, 14);
            this.buttonsTable.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.buttonsTable.Name = "buttonsTable";
            this.buttonsTable.RowCount = 1;
            this.buttonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonsTable.Size = new System.Drawing.Size(220, 12);
            this.buttonsTable.TabIndex = 3;
            // 
            // previousButton
            // 
            this.previousButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previousButton.FlatAppearance.BorderSize = 0;
            this.previousButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.previousButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.previousButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousButton.ForeColor = System.Drawing.Color.White;
            this.previousButton.Location = new System.Drawing.Point(0, 0);
            this.previousButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(73, 12);
            this.previousButton.TabIndex = 0;
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.PreviousButtonOnClick);
            // 
            // playPauseButton
            // 
            this.playPauseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playPauseButton.FlatAppearance.BorderSize = 0;
            this.playPauseButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.playPauseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.playPauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playPauseButton.ForeColor = System.Drawing.Color.White;
            this.playPauseButton.Location = new System.Drawing.Point(73, 0);
            this.playPauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.Size = new System.Drawing.Size(73, 12);
            this.playPauseButton.TabIndex = 1;
            this.playPauseButton.UseVisualStyleBackColor = true;
            this.playPauseButton.Click += new System.EventHandler(this.PlayPauseButtonOnClick);
            // 
            // nextButton
            // 
            this.nextButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextButton.FlatAppearance.BorderSize = 0;
            this.nextButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextButton.ForeColor = System.Drawing.Color.White;
            this.nextButton.Location = new System.Drawing.Point(146, 0);
            this.nextButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(74, 12);
            this.nextButton.TabIndex = 2;
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.NextButtonOnClick);
            // 
            // audioProgress
            // 
            this.mainTable.SetColumnSpan(this.audioProgress, 2);
            this.audioProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioProgress.Location = new System.Drawing.Point(0, 28);
            this.audioProgress.Margin = new System.Windows.Forms.Padding(0);
            this.audioProgress.Name = "audioProgress";
            this.audioProgress.Progress = 100D;
            this.audioProgress.Size = new System.Drawing.Size(250, 2);
            this.audioProgress.TabIndex = 3;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.mainTable);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(250, 40);
            this.MinimumSize = new System.Drawing.Size(250, 30);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(250, 30);
            this.mainTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).EndInit();
            this.buttonsTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.PictureBox albumArt;
        private AudioBand.NowPlayingDisplay nowPlayingText;
        private System.Windows.Forms.TableLayoutPanel buttonsTable;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Button nextButton;
        private AudioBand.EnhancedProgressBar audioProgress;
    }
}
