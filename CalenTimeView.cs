using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalenTrack {
	/// <summary>
	/// Manages mainForm's timeView
	/// </summary>
	class CalenTimeView {
		public static void update(MainForm form = null) {
			if (form == null) form = MainForm.inst;
			var config = CalenCore.config;
			var sorted = CalenApp.sorted;

			var selectStart = CalenCore.selectStart;
			if (selectStart != -1) {
				foreach (var app in CalenApp.list) {
					app.selectedActiveTime = 0;
					app.selectedIdleTime = 0;
				}
				var selectEnd = CalenCore.selectEnd;
				var hourStart = 0;
				var hourEnd = CalenHour.ticksPerHour;
				var appTotal = CalenApp.appTotal;
				foreach (var hour in CalenCore.hours) {
					var selhStart = Math.Max(hourStart, selectStart) - hourStart;
					var selhEnd = Math.Min(hourEnd, selectEnd + 1) - hourStart;
					for (var i = selhStart; i < selhEnd; i++) {
						var appid = hour.appIds[i];
						var app = CalenApp.list[CalenAppId.getIndex(appid)];
						if (CalenAppId.isIdle(appid)) {
							app.selectedIdleTime += CalenHour.tickRate;
							appTotal.selectedIdleTime += CalenHour.tickRate;
						} else {
							app.selectedActiveTime += CalenHour.tickRate;
							appTotal.selectedActiveTime += CalenHour.tickRate;
						}
					}
					hourStart += CalenHour.ticksPerHour;
					hourEnd += CalenHour.ticksPerHour;
				}
				sorted.Sort((a, b) => {
					var at = a.selectedActiveTime;
					var bt = b.selectedActiveTime;
					return at < bt ? 1 : (at > bt ? -1 : a.name.CompareTo(b.name));
				});
			} else sorted.Sort((a, b) => {
				var at = a.activeTime;
				var bt = b.activeTime;
				return at < bt ? 1 : (at > bt ? -1 : a.name.CompareTo(b.name));
			});

			var n = Math.Min((int)form.maxAppRows.Value, sorted.Count);
			var items = new ListViewItem[n + 2];
			var selectedItems = new List<ListViewItem>();
			var rest = config.colors.rest.ToString();
			var selectedIndices = new List<int>();
			for (int i = 0; i <= n + 1; i++) {
				var app = i < n ? sorted[i] : i == n ? CalenApp.appInactive : CalenApp.appTotal;
				if (app.isHighlighted) selectedIndices.Add(i);
				//
				var item = new ListViewItem(app.name);
				if (app.index >= 3) {
					if (app.color.active != Color.Empty) {
						item.ImageKey = app.colorKey;
					} else item.ImageKey = rest;
				}
				double activeTime, idleTime;
				if (selectStart != -1) {
					activeTime = app.selectedActiveTime;
					idleTime = app.selectedIdleTime;
				} else {
					activeTime = app.activeTime;
					idleTime = app.idleTime;
				}
				item.SubItems.Add(Helpers.TimeTools.toString(activeTime));
				item.SubItems.Add(Helpers.TimeTools.toString(idleTime));
				item.SubItems.Add("" + app.index);
				//if (activeApps.ContainsKey(app.index)) selectedItems.Add(item);
				items[i] = item;
			}

			var timeView = form.timeView;
			timeView.Items.Clear();
			timeView.Items.AddRange(items);
			timeView.SelectedIndices.Clear();
			foreach (var i in selectedIndices) timeView.SelectedIndices.Add(i);
		}
	}
}
