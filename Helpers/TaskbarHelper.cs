using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalenTrack.Helpers {
	class TaskbarHelper {
		public static void SetBreakState(MainForm self, double idleTime, double breakTime) {
			if (breakTime < 0) {
				self.Text = self.origTitle;
				self.Icon = self.origIcon;
			} else {
				self.Text = CalenCore.config.breakText;
				self.Icon = self.breakIcon;
			}
			try {
				var tm = TaskbarManager.Instance;
				if (breakTime < 0) {
					tm.SetProgressState(TaskbarProgressBarState.NoProgress);
				} else {
					switch (CalenCore.config.breakTaskbarKind) {
						case "error": tm.SetProgressState(TaskbarProgressBarState.Error); break;
						case "paused": tm.SetProgressState(TaskbarProgressBarState.Paused); break;
						default: tm.SetProgressState(TaskbarProgressBarState.Normal); break;
					}
					tm.SetProgressValue((int)(breakTime - idleTime), (int)breakTime);
				}
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}
	}
}
