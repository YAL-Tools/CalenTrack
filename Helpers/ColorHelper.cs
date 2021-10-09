using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace CalenTrack {
	class ColorHelper {
		private static Regex rxRGB = new Regex("^rgb\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*\\)");
		private static Regex rxRGBA = new Regex("^rgba\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*\\)");
		private static Regex rxHex = new Regex("^(?:#|0x)([0-9a-fA-F]{6})$");
		private static Regex rxVar = new Regex("^\\$\\w+$");
		public static Dictionary<string, Color> colorVars = new Dictionary<string, Color>();
		public static double idleAlpha = 0.7;
		public static Color idleOf(Color c) {
			if (c == Color.Empty) return c;
			return Color.FromArgb((int)(c.A * idleAlpha), c.R, c.G, c.B);
			// Color.FromArgb(c.R / 2, c.G / 2, c.B / 2);
		}
		public static bool TryParse(string val, out Color result) {
			if (rxRGB.IsMatch(val)) {
				var mt = rxRGB.Match(val);
				result = Color.FromArgb(
					int.Parse(mt.Groups[1].Value),
					int.Parse(mt.Groups[2].Value),
					int.Parse(mt.Groups[3].Value)
				);
			} else if (rxRGBA.IsMatch(val)) {
				var mt = rxRGBA.Match(val);
				result = Color.FromArgb(
					int.Parse(mt.Groups[4].Value),
					int.Parse(mt.Groups[1].Value),
					int.Parse(mt.Groups[2].Value),
					int.Parse(mt.Groups[3].Value)
				);
			} else if (rxHex.IsMatch(val)) {
				var hex = uint.Parse(rxHex.Match(val).Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
				if (val.Length < 8) hex |= 0xff000000;
				result = Color.FromArgb((int)hex);
			} else if (rxVar.IsMatch(val)) {
				if (colorVars.TryGetValue(val, out result)) return true;
				result = Color.Empty;
				return false;
			} else {
				result = Color.Empty;
				return false;
			}
			return true;
		}
	}
}
