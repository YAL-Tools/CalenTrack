using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CalenTrack {
	public class CalenColor {
		public Color active;
		public Color idle;
		public CalenColor(Color color) {
			active = color;
			idle = ColorHelper.idleOf(color);
		}
		public Color getColor(bool isActive) {
			return isActive ? active : idle;
		}
		override public string ToString() {
			return active.ToString();
		}
	}
}
