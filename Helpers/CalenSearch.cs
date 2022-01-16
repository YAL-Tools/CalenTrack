using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CalenTrack.Helpers;
using System.Drawing;
using System.Windows.Forms;

namespace CalenTrack.Helpers {
	class CalenSearch {
		static Dictionary<IntPtr, CalenHwnd> hwndCache = new Dictionary<IntPtr, CalenHwnd>();
		static Dictionary<string, string> fileVersionNames = new Dictionary<string, string>();
		static Regex rxRuleRes = new Regex("\\$(t|p)(\\d+)");

		public static uint GetCurrent(double dt, out bool hasFocus) {
			var config = CalenCore.config;

			var hwnd = Win32.GetForegroundRootWindow();
			hasFocus = hwnd == MainForm.inst.Handle;
			string path;
			if (!hwndCache.TryGetValue(hwnd, out var hwndc)) {
				path = Win32.GetModuleFileNameFromHwnd(hwnd);
				hwndc = new CalenHwnd(hwnd, path, CalenCore.lastTick);
				hwndCache[hwnd] = hwndc;
			} else {
				path = hwndc.path;
				hwndc.time = CalenCore.lastTick;
			}
			var title = Win32.GetWindowCaption(hwnd);
			//
			string tag = null;
			Color color = Color.Empty;
			// patches here
			CalenApp app = null;
			ConfigRule matchedRule = null;
			foreach (var rule in config.rules) {
				if (rule.incPath != null && !path.Contains(rule.incPath)) continue;
				if (rule.incTitle != null && !title.Contains(rule.incTitle)) continue;
				//
				Match mtPath = null;
				if (rule.rxPath != null) {
					if (path == null) continue;
					mtPath = rule.rxPath.Match(path);
					if (!mtPath.Success) continue;
				}
				Match mtTitle = null;
				if (rule.rxTitle != null) {
					mtTitle = rule.rxTitle.Match(title);
					if (!mtTitle.Success) continue;
				}
				if (rule.label != null) {
					tag = rxRuleRes.Replace(rule.label, (Match mt) => {
						try {
							int i = int.Parse(mt.Groups[2].Value);
							switch (mt.Groups[1].Value) {
								case "t": return mtTitle.Groups[i].Value;
								case "p": return mtPath.Groups[i].Value;
								default: return mt.Value;
							}
						} catch (Exception e) {
							if (!rule.hasError) {
								MessageBox.Show("Match error!"
									+ "\nTitle: " + title
									+ "\nPath: " + path
									+ "\nError: " + e
								);
								rule.hasError = true;
							}
							return mt.Value;
						}
					});
				}
				if (rule.color != Color.Empty) color = rule.color;
				matchedRule = rule;
				break;
			}
			//
			var log = false;
			if (app == null && tag == null) {
				if (path == null || path == "") {
					tag = Win32.GetModuleBaseNameFromHwnd(hwnd);
					if (log) Console.WriteLine("base: " + tag);
					if (tag == "") tag = title;
					if (tag == "") tag = path;
				} else if (!fileVersionNames.TryGetValue(path, out tag)) {
					try {
						var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
						tag = info.FileDescription;
						if (log) Console.WriteLine("desc: " + tag);
						if (tag == "") tag = info.FileName;
						fileVersionNames[path] = tag;
					} catch (Exception) {
						tag = title;
					}
				}
			}
			//
			if (log) {
				Console.WriteLine("hwnd: " + hwnd);
				Console.WriteLine("title: `" + title + "`");
				Console.WriteLine("path: " + path);
				Console.WriteLine("tag: " + tag);
			}
			//
			if (app == null) {
				if (tag == null || tag == "") {
					app = CalenApp.appUnknown;
				} else {
					app = CalenApp.findOrCreate(tag, color, matchedRule);
				}
			}
			double idleTime = ((double)Win32.GetIdleTime()) / 1000;
			var idle = idleTime > config.timeTillIdle;
			if (!idle && !CalenCore.takeABreak && config.timeTillBreak > 0) {
				CalenCore.timeSinceBreak += dt;
				if (CalenCore.timeSinceBreak > config.timeTillBreak) {
					CalenCore.takeABreak = true;
					TaskbarHelper.SetBreakState(MainForm.inst, 100, 100);
				}
			}
			if (idleTime >= CalenHour.tickRate && CalenCore.timeSinceBreak > 0) {
				var breakTime = config.breakTime;
				if (idleTime > breakTime) {
					CalenCore.timeSinceBreak = 0;
					if (CalenCore.takeABreak) {
						CalenCore.takeABreak = false;
						TaskbarHelper.SetBreakState(MainForm.inst, 0, -1);
					}
				} else if (CalenCore.takeABreak) {
					TaskbarHelper.SetBreakState(MainForm.inst, idleTime, breakTime);
				}
			}
			if (idle) {
				CalenApp.appTotal.idleTime += dt;
				app.idleTime += dt;
			} else {
				CalenApp.appTotal.activeTime += dt;
				app.activeTime += dt;
			}
			uint appid = app.index;
			if (idle) appid |= CalenAppId.bitsIdle;
			return appid;
		}
	}

	public class CalenHwnd {
		public string path;
		public double time;
		public IntPtr hwnd;
		public CalenHwnd(IntPtr hwnd, string path, double time) {
			this.hwnd = hwnd;
			this.path = path;
			this.time = time;
		}
	}
}
