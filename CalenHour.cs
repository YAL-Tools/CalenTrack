using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CalenTrack {
	/// <summary>
	/// 
	/// </summary>
	class CalenHour {
		public const int tickRate = 5;
		public const int width = 24;
		public const int height = 30;
		public const int ticksPerHour = width * height;

		public Bitmap bitmap;
		public uint[] appIds = new uint[ticksPerHour];
		public int count = 0;

		public CalenHour() {
			bitmap = new Bitmap(width, height);
		}
		public bool add(uint appid) {
			if (count >= ticksPerHour) return false;
			var i = count++;
			appIds[i] = appid;
			var app = CalenApp.get(appid);
			var color = app.getColor(appid);
			bitmap.SetPixel(i % width, i / width, color);
			//bitmap.SetPixel(0, 0, Color.Red);
			return true;
		}
	}
}
