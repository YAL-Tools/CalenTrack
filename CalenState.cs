using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalenTrack.Helpers;
using Newtonsoft.Json;

namespace CalenTrack {
	class CalenState {
		const string appidSig = "CalenTrack:appids";

		public string type = "CalenTrack:state";
		public int version = 1000;
		public DateTime startTime;
		public List<CalenAppState> apps = new List<CalenAppState>();
		CalenState() { }

		public static void save(string name = "state") {
			var state = new CalenState();
			state.startTime = CalenCore.startTime;
			foreach (var app in CalenApp.list) {
				var appState = new CalenAppState();
				appState.name = app.name;
				appState.color = app.color.active.ToArgb().ToString("X");
				if (app.rule != null) {
					if (app.rule.name != null) {
						appState.rule = "@" + app.rule.name;
					} else if (app.rule.label != null) {
						appState.rule = "#" + app.rule.label;
					}
				}
				state.apps.Add(appState);
			}
			File.WriteAllText(name + ".json", JsonConvert.SerializeObject(state, Formatting.Indented));

			var appidFile = new BinaryWriter(File.OpenWrite(name + ".appids"));
			appidFile.Write(appidSig);
			appidFile.Write(1000);
			appidFile.Write((CalenCore.hours.Count - 1) * CalenHour.ticksPerHour + CalenCore.latestHour.count);
			foreach (var hour in CalenCore.hours) {
				var n = hour.count;
				for (var i = 0; i < n; i++) {
					appidFile.Write(hour.appIds[i]);
				}
			}
			appidFile.Close();
		}
		public static void load(string name = "state") {
			var state = JsonConvert.DeserializeObject<CalenState>(File.ReadAllText(name + ".json"));
			CalenCore.startTime = state.startTime;
			CalenApp.clear();
			var config = CalenCore.config;
			foreach (var appState in state.apps) {
				var ruleId = appState.rule;
				ConfigRule rule;
				if (ruleId == null) {
					rule = null;
				} else if (ruleId.StartsWith("@")) {
					var ruleName = ruleId.Substring(1);
					rule = config.rules.Find(r => r.name == ruleName);
				} else if (ruleId.StartsWith("#")) {
					var ruleLabel = ruleId.Substring(1);
					rule = config.rules.Find(r => r.label == ruleLabel);
				} else rule = null;
				Color color;
				if (appState.color != null) {
					var colorUint = uint.Parse(appState.color, System.Globalization.NumberStyles.HexNumber);
					color = Color.FromArgb((int)colorUint);
				} else color = Color.Transparent;
				var app = CalenApp.findOrCreate(appState.name, color, rule);
			}
			CalenApp.relink();

			CalenCore.hours.Clear();
			CalenHour latestHour = null;
			var appidFile = new BinaryReader(File.OpenRead(name + ".appids"));
			var signature = appidFile.ReadString();
			var version = appidFile.ReadInt32(); // version
			var count = appidFile.ReadInt32();
			var appTotal = CalenApp.appTotal;
			for (var i = 0; i < count; i++) {
				if (i % CalenHour.ticksPerHour == 0) {
					latestHour = new CalenHour();
					CalenCore.hours.Add(latestHour);
				}
				var appid = appidFile.ReadUInt32();
				latestHour.add(appid);
				var app = CalenAppId.getApp(appid);
				if (CalenAppId.isIdle(appid)) {
					app.idleTime += CalenHour.tickRate;
					appTotal.idleTime += CalenHour.tickRate;
				} else {
					app.activeTime += CalenHour.tickRate;
					appTotal.activeTime += CalenHour.tickRate;
				}
			}
			CalenCore.latestHour = latestHour;
			appidFile.Close();
			CalenCore.lastTick = (count - TimeTools.getTicksSinceHourStart(state.startTime)) * CalenHour.tickRate;
		}
	}

	class CalenAppState {
		public string name;
		public string color;
		public string rule;
		public CalenAppState() { }
	}
}
