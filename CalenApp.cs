using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CalenTrack {
	class CalenApp {
		public string name;
		public CalenColor color;
		public string colorKey;
		public uint index;

		public double activeTime = 0;
		public double idleTime = 0;

		public double selectedActiveTime = 0;
		public double selectedIdleTime = 0;
		public bool isHighlighted = false;

		public ConfigRule rule;
		public CalenApp(string name, uint index, Color color, ConfigRule rule) {
			this.name = name;
			this.index = index;
			this.rule = rule;
			this.color = new CalenColor(color);
			this.colorKey = color.ToString();
		}

		public Color getColor(uint appid) {
			if (color.active.A > 0 || this == appInactive) {
				return CalenAppId.isIdle(appid) ? color.idle : color.active;
			} else {
				return CalenAppId.isIdle(appid) ? CalenCore.config.colors.rest.idle : CalenCore.config.colors.rest.active;
			}
		}
		// statics:

		public static CalenApp appInactive, appUnknown, appTotal, appMarker;

		public static List<CalenApp> list = new List<CalenApp>();
		public static List<CalenApp> sorted = new List<CalenApp>();
		public static List<CalenApp> highlighted = new List<CalenApp>();
		public static void clearHighlight() {
			foreach (var app in highlighted) app.isHighlighted = false;
			highlighted.Clear();
		}
		public static Dictionary<string, CalenApp> map = new Dictionary<string, CalenApp>();

		public static void clear() {
			list.Clear();
			sorted.Clear();
			map.Clear();
		}

		public static CalenApp get(uint appid) {
			return list[CalenAppId.getIndex(appid)];
		}

		public static CalenApp findOrCreate(string name, Color? color = null, ConfigRule rule = null) {
			if (!map.TryGetValue(name, out var app)) {
				app = new CalenApp(name,
					(uint)list.Count,
					color != null ? (Color)color : Color.Empty,
					rule);
				map[name] = app;
				list.Add(app);
				sorted.Add(app);
			}
			return app;
		}

		public static void relink() {
			appInactive = findOrCreate("Inactive");
			sorted.Remove(appInactive);
			appUnknown = findOrCreate("Unknown");
			appTotal = findOrCreate("Total");
			sorted.Remove(appTotal);
			appMarker = findOrCreate("Marker", CalenCore.config.colors.marker);
		}

		public static void reset() {
			clear();
			relink();
		}
	}
}
