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
            this.albumArt = new System.Windows.Forms.PictureBox();
            this.previousButton = new System.Windows.Forms.Button();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.audioProgress = new AudioBand.EnhancedProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).BeginInit();
            this.SuspendLayout();
            // 
            // albumArt
            // 
            this.albumArt.Location = new System.Drawing.Point(0, 0);
            this.albumArt.Margin = new System.Windows.Forms.Padding(0);
            this.albumArt.Name = "albumArt";
            this.albumArt.Size = new System.Drawing.Size(30, 28);
            this.albumArt.TabIndex = 0;
            this.albumArt.TabStop = false;
            this.albumArt.MouseLeave += new System.EventHandler(this.AlbumArtOnMouseLeave);
            this.albumArt.MouseHover += new System.EventHandler(this.AlbumArtOnMouseHover);
            // 
            // previousButton
            // 
            this.previousButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
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
            this.playPauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
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
            this.nextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
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
            this.audioProgress.Location = new System.Drawing.Point(0, 28);
            this.audioProgress.Margin = new System.Windows.Forms.Padding(0);
            this.audioProgress.Name = "audioProgress";
            this.audioProgress.Size = new System.Drawing.Size(250, 2);
            this.audioProgress.TabIndex = 3;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.albumArt);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.playPauseButton);
            this.Controls.Add(this.audioProgress);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(250, 30);
            this.MinimumSize = new System.Drawing.Size(250, 30);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(250, 30);
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox albumArt;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Button nextButton;
        private AudioBand.EnhancedProgressBar audioProgress;
    }
}
