
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Luktac {
	public delegate void ProgressEventHandler(object sender, double precent);
	public delegate void CompleteEventHandler(object sender, bool success);

	public class MediaConvert {
		public event ProgressEventHandler Progress;
		public event CompleteEventHandler Completed;

		private Process process = null;
		private double totalTime;
		private bool encodeStarted;

		protected virtual void OnProgress(double progress) {
			if (this.Progress != null) {
				this.Progress(this, progress);
			}
		}

		protected virtual void OnComplete(bool success) {
			if (this.Completed != null) {
				this.Completed(this, success);
			}
		}
        // ffmpeg encoding function and path location
		public void Convert(FileInfo src, FileInfo dest) {
			string exec = Path.Combine(Application.StartupPath, "tools\\ffmpeg.exe");
			string args = "-loglevel info -y -i \"" + src.FullName + "\" -vcodec mpeg4 -b 4000k \"" + dest.FullName + "\"";

			Process p = new Process();
			p.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
			p.Exited += new EventHandler(process_Exited);
			p.EnableRaisingEvents = true;
			// Redirect the output stream.
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.FileName = exec;
			p.StartInfo.Arguments = args;
			try {
				if (p.Start()) {
					p.BeginErrorReadLine();
					this.process = p;
				}
			} catch { this.terminate(); }
		}

		private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
			string line = e.Data;
			if (!string.IsNullOrEmpty(line)) {
				line = line.Trim();

				if (line.StartsWith("duration", StringComparison.OrdinalIgnoreCase)) {
					// capture the duration, so that we can calculate the "progress"
					// format is like below
					// "Duration: 00:00:30.09, start: 8.000000, bitrate: 6977 kb/s"
					Regex rx = new Regex(@"duration:\s(?<duration>\d\d:\d\d:\d\d.\d\d),\sstart(.*),\sbitrate:\s(?<bitrate>\d*)\skb/s", RegexOptions.IgnoreCase);

					Match m = rx.Match(line);
					if (m.Success) {
						TimeSpan t = new TimeSpan();
						TimeSpan.TryParse(m.Groups["duration"].Value, out t);
						this.totalTime = t.TotalSeconds;
					}
				} else if (line.StartsWith("frame=", StringComparison.OrdinalIgnoreCase)) {
					// the ffmpeg are processing
					// "frame=   25 fps=  0 q=31.0 size=       0kB time=0.87 bitrate=   0.4kbits/s"
					Regex rx = new Regex(@"time=(?<time>\d*.\d*)\s", RegexOptions.IgnoreCase);
					Match m = rx.Match(line);
					if (m.Success) {
						double time = 0;
						double.TryParse(m.Groups["time"].Value, out time);
						double progress = time / this.totalTime;
						this.OnProgress(progress);
					}
				} else if(line.Equals("Press [q] to stop encoding", StringComparison.OrdinalIgnoreCase)){
					this.encodeStarted = true;
				}

			}
		}

		public void terminate() {
			if (this.process != null) {
				this.process.CancelErrorRead();
				if (!this.process.HasExited) this.process.Kill();
			}
			this.OnComplete(this.encodeStarted);
		}

		private void process_Exited(object sender, EventArgs e) {
			this.terminate();
		}

	} // eoc
}
