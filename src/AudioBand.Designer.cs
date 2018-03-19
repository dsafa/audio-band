namespace AudioBand
{
    partial class AudioBand
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
            this.nowPlayingText = new System.Windows.Forms.Label();
            this.buttonsTable = new System.Windows.Forms.TableLayoutPanel();
            this.previous = new System.Windows.Forms.Button();
            this.playPause = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.mainTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).BeginInit();
            this.buttonsTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.AutoSize = true;
            this.mainTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTable.Controls.Add(this.albumArt, 0, 0);
            this.mainTable.Controls.Add(this.nowPlayingText, 1, 0);
            this.mainTable.Controls.Add(this.buttonsTable, 1, 1);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Margin = new System.Windows.Forms.Padding(0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 3;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.mainTable.Size = new System.Drawing.Size(300, 40);
            this.mainTable.TabIndex = 0;
            // 
            // albumArt
            // 
            this.albumArt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.albumArt.Location = new System.Drawing.Point(0, 0);
            this.albumArt.Margin = new System.Windows.Forms.Padding(0);
            this.albumArt.Name = "albumArt";
            this.mainTable.SetRowSpan(this.albumArt, 2);
            this.albumArt.Size = new System.Drawing.Size(30, 38);
            this.albumArt.TabIndex = 0;
            this.albumArt.TabStop = false;
            // 
            // nowPlayingText
            // 
            this.nowPlayingText.AutoSize = true;
            this.nowPlayingText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nowPlayingText.ForeColor = System.Drawing.Color.White;
            this.nowPlayingText.Location = new System.Drawing.Point(30, 0);
            this.nowPlayingText.Margin = new System.Windows.Forms.Padding(0);
            this.nowPlayingText.Name = "nowPlayingText";
            this.nowPlayingText.Size = new System.Drawing.Size(270, 19);
            this.nowPlayingText.TabIndex = 2;
            this.nowPlayingText.Text = "label1";
            this.nowPlayingText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonsTable
            // 
            this.buttonsTable.ColumnCount = 3;
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.buttonsTable.Controls.Add(this.previous, 0, 0);
            this.buttonsTable.Controls.Add(this.playPause, 1, 0);
            this.buttonsTable.Controls.Add(this.next, 2, 0);
            this.buttonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsTable.Location = new System.Drawing.Point(30, 19);
            this.buttonsTable.Margin = new System.Windows.Forms.Padding(0);
            this.buttonsTable.Name = "buttonsTable";
            this.buttonsTable.RowCount = 1;
            this.buttonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonsTable.Size = new System.Drawing.Size(270, 19);
            this.buttonsTable.TabIndex = 3;
            // 
            // previous
            // 
            this.previous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previous.FlatAppearance.BorderSize = 0;
            this.previous.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.previous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previous.ForeColor = System.Drawing.Color.White;
            this.previous.Location = new System.Drawing.Point(0, 0);
            this.previous.Margin = new System.Windows.Forms.Padding(0);
            this.previous.Name = "previous";
            this.previous.Size = new System.Drawing.Size(89, 19);
            this.previous.TabIndex = 0;
            this.previous.Text = "previous";
            this.previous.UseVisualStyleBackColor = true;
            // 
            // playPause
            // 
            this.playPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playPause.FlatAppearance.BorderSize = 0;
            this.playPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.playPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playPause.ForeColor = System.Drawing.Color.White;
            this.playPause.Location = new System.Drawing.Point(89, 0);
            this.playPause.Margin = new System.Windows.Forms.Padding(0);
            this.playPause.Name = "playPause";
            this.playPause.Size = new System.Drawing.Size(89, 19);
            this.playPause.TabIndex = 1;
            this.playPause.Text = "button2";
            this.playPause.UseVisualStyleBackColor = true;
            // 
            // next
            // 
            this.next.Dock = System.Windows.Forms.DockStyle.Fill;
            this.next.FlatAppearance.BorderSize = 0;
            this.next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next.ForeColor = System.Drawing.Color.White;
            this.next.Location = new System.Drawing.Point(178, 0);
            this.next.Margin = new System.Windows.Forms.Padding(0);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(92, 19);
            this.next.TabIndex = 2;
            this.next.Text = "button3";
            this.next.UseVisualStyleBackColor = true;
            // 
            // AudioBand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.mainTable);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(300, 40);
            this.MinimumSize = new System.Drawing.Size(300, 30);
            this.Name = "AudioBand";
            this.Size = new System.Drawing.Size(300, 40);
            this.mainTable.ResumeLayout(false);
            this.mainTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArt)).EndInit();
            this.buttonsTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.PictureBox albumArt;
        private System.Windows.Forms.Label nowPlayingText;
        private System.Windows.Forms.TableLayoutPanel buttonsTable;
        private System.Windows.Forms.Button previous;
        private System.Windows.Forms.Button playPause;
        private System.Windows.Forms.Button next;
    }
}
