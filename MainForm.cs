using CalenTrack.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalenTrack {
	public partial class MainForm : Form {
		public static MainForm inst;

		public string origTitle;
		public Icon origIcon;
		public Icon breakIcon;

		public MainForm() {
			inst = this;
			InitializeComponent();
			Win32.SetDoubleBuffering(timeView, true);
			DoubleBuffered = true;
		}

		private void MainForm_Load(object sender, EventArgs e) {
			var ListView_doubleBuffered = typeof(ListView).GetProperty("DoubleBuffered");
			if (ListView_doubleBuffered != null) ListView_doubleBuffered.SetValue(timeView, true);

			origTitle = Text;
			origIcon = Icon;
			var breakIconPath = "breakTime.ico";
			if (System.IO.File.Exists(breakIconPath)) {
				breakIcon = new Icon(breakIconPath);
			} else breakIcon = origIcon;

			timeView.Columns.Add("Time", 50, HorizontalAlignment.Left);
			timeView.Columns.Add("Idle", 50, HorizontalAlignment.Left);
			var nameCol = timeView.Columns.Add("Name", -2, HorizontalAlignment.Left);

			timeView.Columns.Remove(nameCol);
			timeView.Columns.Insert(0, nameCol);
			timeView.AllowColumnReorder = true;

			CalenCore.init();
			CalenDraw.init();

			tickTimer.Interval = (int)(CalenHour.tickRate * 0.7);
			tickTimer.Start();
			CalenState.load();
		}

		private void tickTimer_Tick(object sender, EventArgs e) {
			CalenCore.timerTick(!isSelecting);
		}

		private void btNew_Click(object sender, EventArgs e) {
			if (CalenCore.lastTick > 15 * 60) {
				switch (MessageBox.Show("Save the current session before starting a new one?", Text, MessageBoxButtons.YesNoCancel)) {
					case DialogResult.Yes:
						if (!Directory.Exists("sessions")) Directory.CreateDirectory("sessions");
						CalenState.save("sessions/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
						break;
					case DialogResult.No:
						break;
					default: return;
				}
			}
			CalenCore.reset();
			CalenDraw.redraw();
		}

		bool isSelecting = false;
		int selectFrom, selectTo;
		void syncSelect(bool finish) {
			var oldStart = CalenCore.selectStart;
			var oldEnd = CalenCore.selectEnd;
			if (selectFrom >= 0 && selectTo >= 0 && (!finish || selectFrom != selectTo)) {
				if (selectTo >= selectFrom) {
					CalenCore.selectStart = selectFrom;
					CalenCore.selectEnd = selectTo;
				} else {
					CalenCore.selectStart = selectTo;
					CalenCore.selectEnd = selectFrom;
				}
			} else {
				CalenCore.selectStart = -1;
				CalenCore.selectEnd = -1;
			}
			if (finish || CalenCore.selectStart != oldStart || CalenCore.selectEnd != oldEnd) {
				CalenDraw.redraw(finish);
			}
		}

		private void canvas_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				var ctrl = (ModifierKeys & Keys.Control) != 0;
				var alt = (ModifierKeys & Keys.Alt) != 0;
				if (!ctrl) CalenApp.clearHighlight();
				//
				var at = CalenDraw.pointToOffset(e.X, e.Y);
				if (at >= 0 && CalenDraw.offsetToHour(at, out var hour, out var index)) {
					var app = CalenAppId.getApp(hour.appIds[index]);
					if (app != null) {
						if (ctrl) {
							if (app.isHighlighted) {
								app.isHighlighted = false;
								CalenApp.highlighted.Remove(app);
							} else {
								app.isHighlighted = true;
								CalenApp.highlighted.Add(app);
							}
						} else {
							app.isHighlighted = true;
							CalenApp.highlighted.Add(app);
						}
					}
				}
				CalenDraw.redraw();
			} else if (e.Button == MouseButtons.Right) {
				selectTo = selectFrom = CalenDraw.pointToOffset(e.X, e.Y);
				isSelecting = selectFrom >= 0;
				syncSelect(false);
			}
		}

		private void canvas_MouseMove(object sender, MouseEventArgs e) {
			var at = CalenDraw.pointToOffset(e.X, e.Y);
			if (isSelecting) {
				selectTo = at;
				if (selectTo >= 0) syncSelect(false);
			} else {
				var app = CalenDraw.offsetToApp(at);
				if (app != null) {
					mouseLabel.Text = app.name;
				} else mouseLabel.Text = "";
			}
		}

		private void canvas_MouseUp(object sender, MouseEventArgs e) {
			if (isSelecting) {
				syncSelect(true);
				isSelecting = false;
			}
		}

		private void timeView_SelectedIndexChanged(object sender, EventArgs e) {
			CalenApp.clearHighlight();
			foreach (ListViewItem item in timeView.SelectedItems) {
				var appid = int.Parse(item.SubItems[3].Text);
				var app = CalenApp.list[appid];
				app.isHighlighted = true;
				CalenApp.highlighted.Add(app);
			}
			CalenDraw.redraw(false);
		}

		private void btAddMarker_Click(object sender, EventArgs e) {
			CalenCore.overrideToMarker = true;
		}

		private void btRefresh_Click(object sender, EventArgs e) {
			CalenDraw.redraw();
		}

		private void btEditConfig_Click(object sender, EventArgs e) {
			Process.Start("config.ini");
		}

		private void btReloadConfig_Click(object sender, EventArgs e) {
			CalenState.save();
			CalenCore.reset();
			CalenState.load();
		}

		private void btEndBreak_Click(object sender, EventArgs e) {
			CalenCore.timeSinceBreak = 0;
			if (CalenCore.takeABreak) {
				CalenCore.takeABreak = false;
				TaskbarHelper.SetBreakState(MainForm.inst, 0, -1);
			}
		}

		private void openSessionDialog_FileOk(object sender, CancelEventArgs e) {
			tickTimer.Enabled = false;
			var path = openSessionDialog.FileName;
			var dot = path.LastIndexOf(".");
			path = path.Substring(0, dot);
			CalenState.load(path);
			CalenDraw.redraw();
		}

		private void toolStripButton1_Click(object sender, EventArgs e) {
			openSessionDialog.ShowDialog();
		}

		private void firstUpdate_Tick(object sender, EventArgs e) {
			if (CalenCore.config == null) return;
			CalenCore.timerTickPost();
			firstUpdate.Enabled = false;
		}
	}
}
