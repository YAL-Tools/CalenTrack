using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CalenTrack.Resources;
using CalenTrack.Helpers;
using System.Drawing;
using System.Windows.Forms;

namespace CalenTrack {
	class CalenCore {
		public static List<CalenHour> hours;
		public static CalenHour latestHour;
		public static DateTime startTime;
		public static double lastTick = 0;

		public static bool takeABreak = false;
		public static double timeSinceBreak = 0;

		public static int selectStart = -1;
		public static int selectEnd = -1;

		public static Config config;

		public static void reset() {
			if (!File.Exists("config.ini")) {
				File.WriteAllText("config.ini", DefaultConfig.get());
			}
			config = new Config("config.ini");

			CalenApp.reset();
			hours = new List<CalenHour>();
			var now = DateTime.Now;
			var hour = new CalenHour();
			hour.count = TimeTools.getTicksSinceHourStart(now);
			hours.Add(hour);
			latestHour = hour;
			startTime = now;
			lastTick = 0;

			var timeView = MainForm.inst.timeView;
			var colors = config.colors;
			var colorThumbs = new ImageList();

			foreach (var c in colors.auto) {
				colorThumbs.Images.Add(c.ToString(), makeThumb(c.active));
			}
			foreach (var rule in config.rules) {
				if (rule.color.IsEmpty) continue;
				colorThumbs.Images.Add(rule.color.ToString(), makeThumb(rule.color));
			}
			foreach (var color in new Color[] { colors.unknown, colors.rest.active }) {
				colorThumbs.Images.Add(color.ToString(), makeThumb(color));
			}
			timeView.SmallImageList = colorThumbs;
		}
		static Bitmap makeThumb(Color color) {
			var bit = new Bitmap(16, 16);
			using (var g = Graphics.FromImage(bit)) g.Clear(color);
			return bit;
		}


		public static void init() {
			reset();
		}

		static void add(uint appid) {
			var ok = latestHour.add(appid);
			if (!ok) {
				var hour = new CalenHour();
				latestHour = hour;
				hours.Add(hour);
				hour.add(appid);
			}
		}
		public static bool overrideToMarker = false;
		public static void timerTickPost(bool updateTimeView = true) {
			var tickRate = CalenHour.tickRate;
			lastTick += tickRate;
			bool hasFocus;
			var appid = Helpers.CalenSearch.GetCurrent(tickRate, out hasFocus);
			if (overrideToMarker) {
				overrideToMarker = false;
				appid = CalenApp.appMarker.index;
			}
			add(appid);
			if (hasFocus) {
				CalenDraw.redraw(updateTimeView);
				CalenState.save();
			}
		}
		public static void timerTick(bool updateTimeView = true) {
			var now = DateTime.Now.Subtract(startTime).TotalSeconds;
			var diff = now - lastTick;
			var tickRate = CalenHour.tickRate;
			if (diff < tickRate) return;

			var skipTime = 0;
			while (diff >= tickRate * 2) {
				add(CalenApp.appInactive.index);
				CalenApp.appInactive.idleTime += tickRate;
				CalenApp.appTotal.idleTime += tickRate;
				diff -= tickRate;
				lastTick += tickRate;
				skipTime += tickRate;
			}
			if (skipTime >= config.timeTillBreak) timeSinceBreak = 0;

			timerTickPost(updateTimeView);
		}
	}
}
