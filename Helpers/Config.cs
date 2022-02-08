using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace CalenTrack {
	public class Config {
		//
		private delegate void Loader(string key, string val);
		private ConfigRule nextRule;
		private int lineNumber = 0;
		// main:
		public bool use24hourTime = true;
		public int scale = 2;
		public int timeTillIdle = 60;
		public int autosaveInterval = 300;
		public double timeTillBreak = 0;
		public double breakTime = 0;
		public string breakText = "Take a break!";
		public string breakTaskbarKind = "normal";
		public List<ConfigRule> rules = new List<ConfigRule>();
		public ConfigColors colors;
		//
		public Config(string path) {
			colors = new ConfigColors(this);
			var lines = File.ReadAllLines(path);
			Loader loader = loadNone;
			var rxSection = new Regex("^\\s*\\[(.+?)\\]");
			var rxPair = new Regex("^\\s*([$\\w]+)\\s*=\\s*(\".*?\"|'.*?'|[^\"'].*|$)");
			lineNumber = 0;
			ColorHelper.colorVars.Clear();
			foreach (var line in lines) {
				if (rxSection.IsMatch(line)) {
					var sctName = rxSection.Match(line).Groups[1].Value;
					switch (sctName) {
						case "main": loader = loadMain; break;
						case "colors": loader = colors.loadConf; break;
						case "rule":
							nextRule = new ConfigRule();
							rules.Add(nextRule);
							loader = loadRule;
							break;
					}
				} else if (rxPair.IsMatch(line)) {
					var mt = rxPair.Match(line);
					var key = mt.Groups[1].Value.ToLower();
					var val = mt.Groups[2].Value;
					if (val.Length > 0) switch (val[0]) {
							case '"':
							case '\'':
								val = val.Substring(1, val.Length - 2);
								break;
							default: val = val.TrimEnd(); break;
						}
					loader(key, val);
				}
				lineNumber += 1;
			}
		}
		//
		private void error(string text, string val) {
			MessageBox.Show(text + "`" + val + "` at line " + lineNumber);
		}
		private void parseBool(string val, out bool target) {
			if (!bool.TryParse(val, out target)) error("Could not parse bool value", val);
		}
		private void parseInt(string val, out int target) {
			if (!int.TryParse(val, out target)) error("Could not parse int value", val);
		}
		private void parseFloat(string val, out double target) {
			if (!double.TryParse(val, NumberStyles.Number, CultureInfo.InvariantCulture, out target)) error("Could not parse float value", val);
		}
		public bool parseColor(string val, out Color result) {
			var z = ColorHelper.TryParse(val, out result);
			if (!z) error("Not a known color format", val);
			return z;
		}
		public bool parseRegex(string val, out Regex result) {
			try {
				result = new Regex(val);
				return true;
			} catch (Exception e) {
				error("Error in Regex: " + e, val);
				result = null;
				return false;
			}
		}
		//
		private void loadNone(string key, string val) {
			//
		}
		private void loadMain(string key, string val) {
			switch (key) {
				case "hourstyle": use24hourTime = val.Trim() != "12"; break;
				case "scale": parseInt(val, out CalenDraw.scale); break;
				case "timetillidle": parseInt(val, out timeTillIdle); break;
				case "autosaveinterval": parseInt(val, out autosaveInterval); break;
				case "idlealpha": parseFloat(val, out ColorHelper.idleAlpha); break;
				case "timetillbreak": parseFloat(val, out timeTillBreak); break;
				case "breaktime": parseFloat(val, out breakTime); break;
				case "breaktext": breakText = val; break;
				case "breaktaskbarkind": breakTaskbarKind = val.ToLower(); break;
			}
		}
		private void loadRule(string key, string val) {
			switch (key) {
				case "name": nextRule.name = val; break;
				case "path": parseRegex(val, out nextRule.rxPath); break;
				case "title": parseRegex(val, out nextRule.rxTitle); break;
				case "pathhas": nextRule.incPath = val; break;
				case "titlehas": nextRule.incTitle = val; break;
				case "label": nextRule.label = val; break;
				case "color": parseColor(val, out nextRule.color); break;
			}
		}
	}
	public class ConfigColors {
		public Config config;
		//
		public List<CalenColor> auto = new List<CalenColor>();
		public CalenColor rest = new CalenColor(Color.SlateGray);
		//
		public Color background = Color.Transparent;
		public Color section = Color.Black;
		public Color outline = Color.Black;
		public Color unknown = Color.Gray;
		public Color marker = Color.Magenta;
		public CalenColor selected = new CalenColor(Color.White);
		public ConfigColors(Config config) {
			this.config = config;
		}
		private bool parseColor(string val, out Color result) {
			return config.parseColor(val, out result);
		}
		public void loadConf(string key, string val) {
			Color col;
			switch (key) {
				case "auto":
					if (parseColor(val, out col)) auto.Add(new CalenColor(col));
					break;
				case "rest":
					if (parseColor(val, out col)) rest = new CalenColor(col);
					break;
				case "background": parseColor(val, out background); break;
				case "section": parseColor(val, out section); break;
				case "outline": parseColor(val, out outline); break;
				case "unknown": parseColor(val, out unknown); break;
				case "marker": parseColor(val, out marker); break;
				case "selected":
					if (parseColor(val, out col)) selected = new CalenColor(col);
					break;
				default:
					if (key.StartsWith("$")) {
						if (parseColor(val, out col)) ColorHelper.colorVars[key] = col;
					}
					break;
			}
		}
	}
	public class ConfigRule {
		public string name = null;
		public string incPath = null;
		public string incTitle = null;
		public Regex rxPath = null;
		public Regex rxTitle = null;
		public string label = null;
		public Color color = Color.Empty;
		public bool hasError = false;
		public ConfigRule() {

		}
		public void assign(ConfigRule value) {
			name = value.name;
			incPath = value.incPath;
			incTitle = value.incTitle;
			rxPath = value.rxPath;
			rxTitle = value.rxTitle;
			label = value.label;
			color = value.color;
		}
	}
}
