using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

namespace CalenTrack {
	public class Win32 {

		#region Idle time
		[DllImport("User32.dll")]
		public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		[DllImport("Kernel32.dll")]
		public static extern uint GetLastError();

		public static uint GetIdleTime() {
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
			GetLastInputInfo(ref lastInPut);
			//
			return ((uint)Environment.TickCount - lastInPut.dwTime);
		}

		public static long GetLastInputTime() {
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
			if (!GetLastInputInfo(ref lastInPut)) {
				throw new Exception(GetLastError().ToString());
			}

			return lastInPut.dwTime;
		}
		#endregion

		#region Window
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetActiveWindow();

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindow(IntPtr hwnd, uint uCmd);

		public static IntPtr GetForegroundRootWindow() {
			var wnd = GetForegroundWindow();
			while (wnd != IntPtr.Zero) {
				var par = GetWindow(wnd, GetWindowConstants.GW_OWNER);
				if (par != IntPtr.Zero) {
					wnd = par;
				} else break;
			}
			return wnd;
		}
		#endregion

		#region Window text
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetWindowTextLength(IntPtr hwnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool IsHungAppWindow(IntPtr hwnd);

		private static Regex rxANR = new Regex("^(.+?) \\(.+?\\)$");
		public static string GetWindowCaption(IntPtr hwnd) {
			var len = GetWindowTextLength(hwnd);
			var sb = new StringBuilder(len + 5);
			GetWindowText(hwnd, sb, len + 2);
			var s = sb.ToString();
			if (IsHungAppWindow(hwnd)) {
				var mt = rxANR.Match(s);
				if (mt.Success) s = mt.Groups[1].Value;
			}
			return s;
		}
		#endregion

		#region Process
		// todo: is it better to have GetWindowThreadProcessId + OpenProcess than Oleacc dependency?
		[DllImport("Oleacc.dll")]
		static extern IntPtr GetProcessHandleFromHwnd(IntPtr hwnd);

		[DllImport("psapi.dll")]
		static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

		[DllImport("psapi.dll")]
		static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);

		public static string GetModuleBaseNameFromHwnd(IntPtr hwnd) {
			var proc = GetProcessHandleFromHwnd(hwnd);
			if (proc == IntPtr.Zero) return null;
			var sb = new StringBuilder(256);
			if (GetModuleBaseName(proc, IntPtr.Zero, sb, 256) == 0) return null;
			return sb.ToString();
		}

		public static string GetModuleFileNameFromHwnd(IntPtr hwnd) {
			var proc = GetProcessHandleFromHwnd(hwnd);
			if (proc == IntPtr.Zero) return "";
			var sb = new StringBuilder(256);
			if (GetModuleFileNameEx(proc, IntPtr.Zero, sb, 256) == 0) return "";
			return sb.ToString();
		}
		#endregion


		public static void SetDoubleBuffering(Control control, bool enable) {
			// https://stackoverflow.com/a/15268338/5578773
			//var method = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
			//method.Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer, enable });
			var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			doubleBufferPropertyInfo.SetValue(control, enable, null);
		}
	}

	public class GetWindowConstants {
		public const uint GW_HWNDFIRST = 0;
		public const uint GW_HWNDLAST = 1;
		public const uint GW_HWNDNEXT = 2;
		public const uint GW_HWNDPREV = 3;
		public const uint GW_OWNER = 4;
		public const uint GW_CHILD = 5;
		public const uint GW_ENABLEDPOPUP = 6;
		public const uint GW_MAX = 6;
	}

	public struct LASTINPUTINFO {
		public uint cbSize;
		public uint dwTime;
	}
}
