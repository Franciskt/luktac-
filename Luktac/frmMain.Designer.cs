namespace Luktac {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.ColumnHeader columnHeader1;
			System.Windows.Forms.ColumnHeader columnHeader2;
			System.Windows.Forms.SplitContainer splitContainer1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.wmp = new AxWMPLib.AxWindowsMediaPlayer();
			this.txt_trace = new System.Windows.Forms.TextBox();
			this.lv_playlist = new System.Windows.Forms.ListView();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.convertFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.protectFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.validateFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.wmp)).BeginInit();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "Name";
			columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Duration";
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(0, 24);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(this.wmp);
			splitContainer1.Panel1.Controls.Add(this.txt_trace);
			splitContainer1.Panel1MinSize = 200;
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(this.lv_playlist);
			splitContainer1.Panel2MinSize = 0;
			splitContainer1.Size = new System.Drawing.Size(534, 338);
			splitContainer1.SplitterDistance = 360;
			splitContainer1.SplitterWidth = 8;
			splitContainer1.TabIndex = 2;
			// 
			// wmp
			// 
			this.wmp.AllowDrop = true;
			this.wmp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wmp.Enabled = true;
			this.wmp.Location = new System.Drawing.Point(0, 0);
			this.wmp.Name = "wmp";
			this.wmp.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmp.OcxState")));
			this.wmp.Size = new System.Drawing.Size(360, 241);
			this.wmp.TabIndex = 2;
			// 
			// txt_trace
			// 
			this.txt_trace.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.txt_trace.Location = new System.Drawing.Point(0, 241);
			this.txt_trace.Multiline = true;
			this.txt_trace.Name = "txt_trace";
			this.txt_trace.Size = new System.Drawing.Size(360, 97);
			this.txt_trace.TabIndex = 3;
			// 
			// lv_playlist
			// 
			this.lv_playlist.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lv_playlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
			this.lv_playlist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lv_playlist.FullRowSelect = true;
			this.lv_playlist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lv_playlist.HideSelection = false;
			this.lv_playlist.LabelWrap = false;
			this.lv_playlist.Location = new System.Drawing.Point(0, 0);
			this.lv_playlist.MultiSelect = false;
			this.lv_playlist.Name = "lv_playlist";
			this.lv_playlist.ShowGroups = false;
			this.lv_playlist.ShowItemToolTips = true;
			this.lv_playlist.Size = new System.Drawing.Size(166, 338);
			this.lv_playlist.TabIndex = 0;
			this.lv_playlist.UseCompatibleStateImageBehavior = false;
			this.lv_playlist.View = System.Windows.Forms.View.Details;
			this.lv_playlist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lv_playlist_KeyDown);
			this.lv_playlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_playlist_MouseDoubleClick);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.DemoToolStripMenuItem});
			this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(534, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertFileToolStripMenuItem,
            this.protectFileToolStripMenuItem,
            this.validateFileToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// convertFileToolStripMenuItem
			// 
			this.convertFileToolStripMenuItem.Enabled = false;
			this.convertFileToolStripMenuItem.Name = "convertFileToolStripMenuItem";
			this.convertFileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.convertFileToolStripMenuItem.Text = "Convert File";
			this.convertFileToolStripMenuItem.Click += new System.EventHandler(this.convertFileToolStripMenuItem_Click);
			// 
			// protectFileToolStripMenuItem
			// 
			this.protectFileToolStripMenuItem.Name = "protectFileToolStripMenuItem";
			this.protectFileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.protectFileToolStripMenuItem.Text = "Sign File";
			this.protectFileToolStripMenuItem.Click += new System.EventHandler(this.protectFileToolStripMenuItem_Click);
			// 
			// validateFileToolStripMenuItem
			// 
			this.validateFileToolStripMenuItem.Name = "validateFileToolStripMenuItem";
			this.validateFileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.validateFileToolStripMenuItem.Text = "Validate File";
			this.validateFileToolStripMenuItem.Click += new System.EventHandler(this.validateFileToolStripMenuItem_Click);
			// 
			// DemoToolStripMenuItem
			// 
			this.DemoToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.DemoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.DemoToolStripMenuItem.Name = "DemoToolStripMenuItem";
			this.DemoToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.DemoToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 362);
			this.Controls.Add(splitContainer1);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(550, 400);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.frmMain_Load);
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.wmp)).EndInit();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem convertFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem protectFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem validateFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DemoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private AxWMPLib.AxWindowsMediaPlayer wmp;
		private System.Windows.Forms.ListView lv_playlist;
		private System.Windows.Forms.TextBox txt_trace;
	}
}

