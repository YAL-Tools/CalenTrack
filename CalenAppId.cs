using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalenTrack {
	class CalenAppId {
		public const uint bitsIndex = ~(1u << 31);
		public const uint bitsIdle = 1u << 31;
 
		public static int getIndex(uint appid) {
			return (int)(appid & bitsIndex);
		}
		public static CalenApp getApp(uint appid) {
			return CalenApp.list[(int)(appid & bitsIndex)];
		}
		public static bool isIdle(uint appid) {
			return (appid & bitsIdle) != 0;
		}
	}
}
