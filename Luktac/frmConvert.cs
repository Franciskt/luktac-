//  canceling of the convertion progress button
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Luktac {
	public partial class frmConvert : Form {
		public frmConvert() {
			InitializeComponent();
		}

		private void frmConvert_Load(object sender, EventArgs e) {
			this.ClientSize = new Size(
				this.btn_cancel.Right + this.btn_cancel.Left,
				this.btn_cancel.Bottom + this.progressBar.Top
				);
		}

		private void btn_cancel_Click(object sender, EventArgs e) {
			DialogResult rst = MessageBox.Show(this,
				Properties.Resources.abort_convert_msg,
				Properties.Resources.abort_convert,
				MessageBoxButtons.YesNo
				);
			if (rst == System.Windows.Forms.DialogResult.Yes) {
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.Close();
			}
		}

		public double Progress{
			set{
				this.progressBar.Value = (int)Math.Floor(value * 100);
			}
		}
	}
}
