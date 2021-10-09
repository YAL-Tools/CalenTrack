using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalenTrack.Helpers {
	class TimeTools {
		public static int getTicksSinceHourStart(DateTime now) {
			return (now.Minute * 60 + now.Second) / CalenHour.tickRate;
		}
		public static string toString(double seconds) {
			//if (seconds < 60) return Math.Ceiling(seconds) + "s";
			var minutes = (long)Math.Ceiling(seconds / 60);
			var hours = minutes / 60;
			if (hours > 0) {
				minutes %= 60;
				return hours + ":" + minutes.ToString().PadLeft(2, '0');
			} else return minutes.ToString();
		}
	}
}
