namespace AudioBand.Settings
{
    public partial class SettingsWindow
    {
        private void InitializeComponent()
        {
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.generalSettingsTab = new MetroFramework.Controls.MetroTabPage();
            this.appearanceTab = new MetroFramework.Controls.MetroTabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.generalSettingsTab);
            this.tabControl.Controls.Add(this.appearanceTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(50, 34);
            this.tabControl.Location = new System.Drawing.Point(20, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 1;
            this.tabControl.Size = new System.Drawing.Size(260, 420);
            this.tabControl.TabIndex = 0;
            this.tabControl.UseSelectable = true;
            // 
            // generalSettingsTab
            // 
            this.generalSettingsTab.HorizontalScrollbarBarColor = true;
            this.generalSettingsTab.HorizontalScrollbarHighlightOnWheel = false;
            this.generalSettingsTab.HorizontalScrollbarSize = 10;
            this.generalSettingsTab.Location = new System.Drawing.Point(4, 38);
            this.generalSettingsTab.Name = "generalSettingsTab";
            this.generalSettingsTab.Size = new System.Drawing.Size(252, 378);
            this.generalSettingsTab.TabIndex = 0;
            this.generalSettingsTab.Text = "General";
            this.generalSettingsTab.VerticalScrollbarBarColor = true;
            this.generalSettingsTab.VerticalScrollbarHighlightOnWheel = false;
            this.generalSettingsTab.VerticalScrollbarSize = 10;
            // 
            // appearanceTab
            // 
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
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage generalSettingsTab;
        private MetroFramework.Controls.MetroTabPage appearanceTab;
    }
}
