using System;
using System.Collections.Generic;
using System.ComponentModel;
// code for the player 
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace Luktac {
	public partial class frmMain : Form {
		private MediaConvert convertor = null;
		private frmConvert progresswindow = null;
		private WMPLib.IWMPPlaylist pl;


		public frmMain() {
			InitializeComponent();

			this.txt_trace.Visible = false;

			this.Text = Properties.Resources.title;

			this.wmp.settings.setMode("loop", false);
			this.wmp.settings.autoStart = false;
			WMPLib.IWMPPlaylistArray plItems;
			plItems = this.wmp.playlistCollection.getByName(Properties.Resources.title);
			if (plItems.count == 0) {
				pl = this.wmp.playlistCollection.newPlaylist(Properties.Resources.title);
			} else {
				pl = plItems.Item(0);
				this.wmp.currentPlaylist = pl;
			}
			this.wmp.settings.autoStart = true;
			this.wmp_PlaylistCollectionChange(this, null);

			this.wmp.PlaylistCollectionChange += new EventHandler(wmp_PlaylistCollectionChange);
			this.wmp.CurrentItemChange += new AxWMPLib._WMPOCXEvents_CurrentItemChangeEventHandler(wmp_CurrentItemChange);
			this.wmp.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wmp_PlayStateChange);

			Control c = this.wmp as Control;
			c.DragDrop += new DragEventHandler(Wmp_DragDrop);
			c.DragEnter += new DragEventHandler(Wmp_DragEnter);

			this.FormClosing += new FormClosingEventHandler(frmMain_FormClosing);
		}

		private void trace(string str) {
			this.txt_trace.AppendText(str + "\r\n");
		}

		bool _validating = false; // function to prevent infinity loop
		WMPPlayState _previousState = WMPPlayState.wmppsUndefined; 
        //  prevention of prompting multiple time when resume playing (pause -> play)
		void wmp_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e) {
			WMPPlayState state = (WMPPlayState)e.newState;
			WMPPlayState old = _previousState;
			_previousState = state;

			this.convertFileToolStripMenuItem.Enabled = false;
			if (state != WMPPlayState.wmppsPlaying) return;
			if (old == WMPPlayState.wmppsPaused) return;

			if (!_validating) {
				_validating = true;
				this.wmp.Ctlcontrols.pause();
				FileInfo file = new FileInfo(this.wmp.Ctlcontrols.currentItem.sourceURL);
				if (ValidateMedia(file)) {
					this.wmp.Ctlcontrols.play();
					this.convertFileToolStripMenuItem.Enabled = true;
				} else {
					this.wmp.Ctlcontrols.stop();
				}
				_validating = false;
			}
		}

		void wmp_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e) {
			if (this.wmp.currentMedia != this.wmp.Ctlcontrols.currentItem) {
				FileInfo file = new FileInfo(this.wmp.currentMedia.sourceURL);

				for (int i = 0; i < this.wmp.currentPlaylist.count; i++) {
					ListViewItem item = this.lv_playlist.Items[i];
					if (this.wmp.currentPlaylist.Item[i].sourceURL.Equals(this.wmp.currentMedia.sourceURL, StringComparison.OrdinalIgnoreCase)) {
						item.Selected = true;
						break;
					}
				}
			}
		}

		private void wmp_PlaylistCollectionChange(object sender, EventArgs e) {
			this.lv_playlist.SuspendLayout();
			this.lv_playlist.Items.Clear();

			for (int i = 0; i < pl.count; i++) {
				ListViewItem item = this.lv_playlist.Items.Add(pl.Item[i].name);
				item.SubItems.Add(pl.Item[i].durationString);
				item.ToolTipText = pl.Item[i].sourceURL;
				if (this.wmp.currentMedia != null) {
					if (pl.Item[i].sourceURL.Equals(this.wmp.currentMedia.sourceURL, StringComparison.OrdinalIgnoreCase)) {
						item.Selected = true;
					}
				}
			}

			this.lv_playlist.ResumeLayout();
		}

		private void Wmp_DragEnter(object sender, DragEventArgs e) {
			e.Effect = System.Windows.Forms.DragDropEffects.None;

			// validete file with extension
			// support only wmv, avi and mp4
			string[] files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
			if (files.Length == 1) { 
                // only support single file dropping
				FileInfo file = new FileInfo(files[0]);
				if (file.Extension.Equals(".wmv", StringComparison.OrdinalIgnoreCase) ||
					file.Extension.Equals(".avi", StringComparison.OrdinalIgnoreCase) ||
						file.Extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase)) {
					e.Effect = System.Windows.Forms.DragDropEffects.All;
				}
			}
		}

		private void Wmp_DragDrop(object sender, DragEventArgs e) {
			// the data are filter by DragEnter event, so should be the data that we need
			// no additional validate required.
			string[] files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
			FileInfo file = new FileInfo(files[0]);

			PlayMedia(file);
		}

		private void PlayMedia(FileInfo file) {
			WMPLib.IWMPMedia m = null;
			pl = this.wmp.currentPlaylist;
			for (int i = 0; i < pl.count; i++) {
				if (pl.Item[i].sourceURL.Equals(file.FullName, StringComparison.OrdinalIgnoreCase)) {
					m = pl.Item[i];
					break;
				}
			}

			if (m == null) {
				m = this.wmp.newMedia(file.FullName);
				pl.appendItem(m);
			};


			this.wmp.Ctlcontrols.playItem(m);
		}

		private bool ValidateMedia(FileInfo file) {
			bool allow = true;
			// checking if the extension is container of our choice (mp4)
			if (file.Extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase)) {
				try {
					switch (MediaHash.Validate(file)) {
						case MediaProtection.Unknow:
							// checking if file not protected 
							if (MessageBox.Show(this, Properties.Resources.err_msg_unknow,
									Properties.Resources.err_title_unknow,
									MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
								allow = false;
							}
							break;
						case MediaProtection.Modified:
							//  checking if the file is modified
							if (MessageBox.Show(this, Properties.Resources.err_msg_modified,
									Properties.Resources.err_title_modified,
									MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
								allow = false;
							}
							break;
						case MediaProtection.Protected:
							// good to go
							break;
					}
				} catch {
					MessageBox.Show(this, Properties.Resources.validate_error_msg,
								Properties.Resources.validate_error,
								MessageBoxButtons.OK, MessageBoxIcon.Error);
					allow = false;
				}
			}

			return allow;
		}

		private void frmMain_Load(object sender, EventArgs e) {

		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
			// stop thread if we are converting...
			if (this.progresswindow != null) {
				this.progresswindow.FormClosing -= frmConvertClosing;
				this.progresswindow.Close();
				this.progresswindow = null;
			}
			if (this.convertor != null) {
				this.convertor.terminate();
			}
		}

		private void convertFileToolStripMenuItem_Click(object sender, EventArgs e) {
			SaveFileDialog save = new SaveFileDialog();
			save.Title = "Save as";
			save.Filter = "Media file|*.mp4";

			DialogResult rst = save.ShowDialog(this);
			if (rst == DialogResult.OK) {
				FileInfo file = new FileInfo(save.FileName);
				bool err = false;
				if (file.Exists) {
					if (file.FullName.Equals(this.wmp.URL, StringComparison.OrdinalIgnoreCase)) {
						// cannot save as playing file
						err = true;
					}
				}
				if (!err) {
					this.convertFileToolStripMenuItem.Enabled = false;

					// start convert file
					this.convertor = new MediaConvert();
					this.convertor.Completed += new CompleteEventHandler(convert_Completed);
					this.convertor.Progress += new ProgressEventHandler(convert_Progress);
					this.convertor.Convert(new FileInfo(this.wmp.currentMedia.sourceURL), file);

					// show progress window
					frmConvert frm = new frmConvert();
					frm.FormClosing += new FormClosingEventHandler(frmConvertClosing);
					Point p = this.wmp.PointToScreen(this.wmp.Location);
					frm.Top = p.Y;
					frm.Left = p.X;
					frm.Owner = this;
					frm.Show();
					this.progresswindow = frm;
				}
			}
		}

		private void frmConvertClosing(object sender, FormClosingEventArgs e) {
			frmConvert frm = sender as frmConvert;
			if (frm != null) {
				if (frm.DialogResult == DialogResult.Cancel) {
					if (this.convertor != null) {
						this.convertor.terminate();
					}
				}
			}
		}

		private void convert_Progress(object sender, double percent) {
			if (this.InvokeRequired) {
				this.Invoke(new Action<object, double>(convert_Progress), sender, percent);
				return;
			}
			this.Text = "Converting: " + (percent * 100).ToString("0.00") + "%";
			if (this.progresswindow != null) {
				this.progresswindow.Progress = percent;
			}
		}

		private void convert_Completed(object sender, bool success) {
			if (this.InvokeRequired) {
				this.Invoke(new Action<object, bool>(convert_Completed), sender, success);
				return;
			}
			this.Text = Properties.Resources.title;
			this.convertor = null;
			this.convertFileToolStripMenuItem.Enabled = true;

			if (this.progresswindow != null) {
				this.progresswindow.FormClosing -= frmConvertClosing;
				this.progresswindow.Close();
				this.progresswindow = null;
			}

			if (!success) {
				MessageBox.Show(this, Properties.Resources.err_msg_convert, Properties.Resources.err_title_convert, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void protectFileToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog open = new OpenFileDialog();
			open.Title = "Protect file";
			open.Filter = "Media file|*.mp4";
			open.Multiselect = false;

			DialogResult rst = open.ShowDialog(this);
			if (rst == DialogResult.OK) {
				FileInfo file = new FileInfo(open.FileName);
				if (!MediaHash.Protect(file)) {
					MessageBox.Show(this,
						Properties.Resources.protect_fail,
						Properties.Resources.protect_title,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					MessageBox.Show(this,
						Properties.Resources.protect_success,
						Properties.Resources.protect_title,
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void validateFileToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog open = new OpenFileDialog();
			open.Title = "Validate file";
			open.Filter = "Media file|*.mp4";
			open.Multiselect = false;

			DialogResult rst = open.ShowDialog(this);
			if (rst == DialogResult.OK) {
				FileInfo file = new FileInfo(open.FileName);
				switch (MediaHash.Validate(file)) {
					case MediaProtection.Unknow:
						MessageBox.Show(this,
							Properties.Resources.validate_msg_unknow,
							Properties.Resources.validate_title_unknow,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					case MediaProtection.Modified:
						MessageBox.Show(this,
							Properties.Resources.validate_msg_modified,
							Properties.Resources.validate_title_modified,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					case MediaProtection.Protected:
						MessageBox.Show(this,
							Properties.Resources.validate_msg_protected,
							Properties.Resources.validate_title_protected,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
				}
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (frmAbout about = new frmAbout()) {
				about.ShowDialog(this);
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog open = new OpenFileDialog();
			open.Title = "Open file";
			open.Filter = "Media file|*.mp4;*.wmv";
			open.Multiselect = false;

			DialogResult rst = open.ShowDialog(this);
			if (rst == DialogResult.OK) {
				FileInfo file = new FileInfo(open.FileName);
				PlayMedia(file);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void lv_playlist_KeyDown(object sender, KeyEventArgs e) {
			ListView lv = sender as ListView;
			if (lv == null) return;

			if (e.KeyCode == Keys.Delete && lv.SelectedIndices.Count > 0) {
				int index = lv.SelectedIndices[0];
				if (index > -1) {
					pl.removeItem(pl.Item[index]);
				}
			}
		}

		private void lv_playlist_MouseDoubleClick(object sender, MouseEventArgs e) {
			ListView lv = sender as ListView;
			if (lv == null) return;

			int index = lv.SelectedIndices[0];
			if (index > -1) {
				FileInfo file = new FileInfo(pl.Item[index].sourceURL);
				this.PlayMedia(file);
			}
		}
	} // eoc
}
