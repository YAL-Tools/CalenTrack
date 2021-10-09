using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CalenTrack {
	class CalenDraw {
		public const int padding = 10;
		public const int labelHeight = 20;
		public static int scale = 6;
		public static PictureBox canvasEl { get => MainForm.inst.canvas; }
		public static Bitmap bitmap;

		static int lastHoursPerRow = 1;
		public static int getHoursPerRow() {
			var width = canvasEl.Width;
			var hoursPerRow = (width - padding) / (scale * CalenHour.width + padding);
			if (hoursPerRow < 1) hoursPerRow = 1;
			return hoursPerRow;
		}

		public static int getHeight() {
			var hours = CalenCore.hours;
			var hoursPerRow = getHoursPerRow();
			var rows = (int)Math.Ceiling((double)hours.Count / hoursPerRow);
			return padding + rows * (padding + labelHeight + scale * CalenHour.height);
		}

		public static Bitmap ensureSize() {
			var canvas = canvasEl;
			var height = getHeight();
			if (bitmap.Width == canvas.Width && bitmap.Height == height) return bitmap;
			bitmap = new Bitmap(canvas.Width, getHeight());
			canvas.Image = bitmap;
			canvas.Height = bitmap.Height;
			return bitmap;
		}

		public static int pointToOffset(int x, int y) {
			var hourWidth = scale * CalenHour.width;
			var hourHeight = scale * CalenHour.height;

			var divX = (padding + hourWidth);
			var divY = (padding + labelHeight + hourHeight);

			var hourCol = (x - padding) / divX;
			var hourRow = (y - padding) / divY;
			if (hourRow < 0 || hourCol < 0) return -1;

			var hourInd = hourCol + hourRow * lastHoursPerRow;
			if (hourInd >= CalenCore.hours.Count) return -1;

			var hourX = padding + hourCol * divX;
			var hourY = padding + hourRow * divY;

			var pixelCol = (x - hourX) / scale;
			var pixelRow = (y - hourY) / scale;
			if (pixelCol < 0 || pixelCol >= CalenHour.width) return -1;
			if (pixelRow < -1 || pixelRow >= CalenHour.height + 2) return -1;
			if (pixelRow < 0) pixelRow = 0;
			if (pixelRow >= CalenHour.height) pixelRow = CalenHour.height - 1;
			var pixelInd = pixelCol + pixelRow * CalenHour.width;

			return pixelInd + hourInd * CalenHour.ticksPerHour;
		}

		public static bool offsetToHour(int offset, out CalenHour hour, out int index) {
			var hourInd = offset / CalenHour.ticksPerHour;
			if (hourInd < 0 || hourInd >= CalenCore.hours.Count) {
				hour = null;
				index = 0;
				return false;
			}
			hour = CalenCore.hours[hourInd];
			index = offset % CalenHour.ticksPerHour;
			return true;
		}

		public static CalenApp offsetToApp(int offset) {
			if (offset < 0) return null;
			if (offsetToHour(offset, out var hour, out var index)) {
				return CalenAppId.getApp(hour.appIds[index]);
			} else return null;
		}

		static Image selectFill = Bitmap.FromFile("diag.png");
		public static void redraw(bool updateTimeView = true) {
			var bitmap = ensureSize();

			var hourWidth = scale * CalenHour.width;
			var hourHeight = scale * CalenHour.height;
			var selectStart = CalenCore.selectStart;
			var selectEnd = CalenCore.selectEnd;
			var hasRangeSelect = selectStart >= 0;
			var hasHighlight = CalenApp.highlighted.Count > 0;

			using (var g = Graphics.FromImage(bitmap)) {
				g.Clear(MainForm.inst.BackColor);
				//g.Clear(Color.Black);

				var hoursPerRow = getHoursPerRow();
				lastHoursPerRow = hoursPerRow;
				int col = 0, row = 0;
				var borderPen = new Pen(Color.White);
				var borderPen2 = new Pen(Color.Black);
				var selectPen = new Pen(Color.Black);
				var selectBrush = new SolidBrush(Color.FromArgb(0x600086F4));//new TextureBrush(selectFill);
				var font = new Font("Verdana", 10);
				var fontBrush = new SolidBrush(Color.Black);
				var currHour = CalenCore.startTime.Hour;
				var fullDays = 0;
				var hourStart = 0;
				var hourEnd = CalenHour.ticksPerHour;
				void fillRectBetween(Brush brush, float x1, float y1, float x2, float y2) {
					g.FillRectangle(brush, x1, y1, x2 - x1 + 1, y2 - y1);
				}
				void drawFillRectBetween(Pen pen, Brush brush, float x1, float y1, float x2, float y2) {
					g.DrawRectangle(pen, x1, y1, x2 - x1, y2 - y1);
					g.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
				}
				foreach (var hour in CalenCore.hours) {
					var hourX = padding + col * (padding + hourWidth);
					var hourY = padding + row * (padding + labelHeight + hourHeight);

					g.DrawRectangle(borderPen2, hourX - 2, hourY - 2, hourWidth + 4, hourHeight + 4);
					g.DrawRectangle(borderPen, hourX - 1, hourY - 1, hourWidth + 2, hourHeight + 2);

					// Draw scaled cached image, but without interpolation/half-pixel offset:
					var pixelOffsetMode = g.PixelOffsetMode;
					var interpolationMode = g.InterpolationMode;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.DrawImage(hour.bitmap, hourX, hourY, hourWidth, hourHeight);
					g.PixelOffsetMode = pixelOffsetMode;
					g.InterpolationMode = interpolationMode;

					//
					if (hasHighlight) {
						var i = 0;
						for (var y = 0; y < CalenHour.height; y++) {
							for (var x = 0; x < CalenHour.width; x++) {
								var app = CalenApp.list[(int)(hour.appIds[i] & CalenAppId.bitsIndex)];
								if (app.isHighlighted) {
									g.FillRectangle(selectBrush, hourX + x * scale, hourY + y * scale, scale, scale);
								}
								i += 1;
							}
						}
					}

					//
					if (!hasRangeSelect) {
						//
					} else if (selectStart <= hourStart && selectEnd >= hourEnd) {
						g.FillRectangle(selectBrush, hourX, hourY, hourWidth, hourHeight);
					} else {
						var hourRight = hourX + hourWidth;
						var hourBottom = hourY + hourHeight;
						var startHere = selectStart >= hourStart && selectStart < hourEnd;
						var tickStart = selectStart - hourStart;
						var tickStartCol = tickStart % CalenHour.width;
						var tickStartRow = tickStart / CalenHour.width;
						var tickStartX = hourX + tickStartCol * scale;
						var tickStartY = hourY + tickStartRow * scale;

						var endHere = selectEnd >= hourStart && selectEnd < hourEnd;
						var tickEnd = selectEnd - hourStart;
						var tickEndCol = tickEnd % CalenHour.width;
						var tickEndRow = tickEnd / CalenHour.width;
						var tickEndX = hourX + (tickEndCol + 1) * scale;
						var tickEndY = hourY + (tickEndRow + 1) * scale;
						void drawEndBorder() {
							fillRectBetween(selectBrush, hourX, tickEndY - scale, tickEndX, tickEndY);
							//         _______
							//  ______x|
							g.DrawLine(selectPen, hourX, tickEndY, tickEndX, tickEndY);
							if (tickEndCol < CalenHour.width - 1) {
								g.DrawLine(selectPen, tickEndX, tickEndY, tickEndX, tickEndY - scale);
								g.DrawLine(selectPen, tickEndX, tickEndY - scale, hourRight, tickEndY - scale);
							}
						}
						if (startHere) {
							if (endHere && tickEndRow == tickStartRow) {
								drawFillRectBetween(selectPen, selectBrush, tickStartX, tickStartY, tickEndX, tickEndY);
							} else if (endHere && tickEndRow == tickStartRow + 1 && tickEndCol < tickStartCol) {
								drawFillRectBetween(selectPen, selectBrush, tickStartX, tickStartY, hourRight, tickStartY + scale);
								drawFillRectBetween(selectPen, selectBrush, hourX, tickEndY - scale, tickEndX, tickEndY);
							} else {
								fillRectBetween(selectBrush, tickStartX, tickStartY, hourRight, tickStartY + scale);
								if (!endHere) {
									fillRectBetween(selectBrush, hourX, tickStartY + scale, hourRight, hourBottom);
								} else if (tickEndY > tickStartY + scale * 2) {
									fillRectBetween(selectBrush, hourX, tickStartY + scale, hourRight, tickEndY - scale);
								}
								//         _______
								//  _______|x
								if (tickStartCol > 0) {
									g.DrawLine(selectPen, hourX, tickStartY + scale, tickStartX, tickStartY + scale);
									g.DrawLine(selectPen, tickStartX, tickStartY, tickStartX, tickStartY + scale);
								}
								g.DrawLine(selectPen, tickStartX, tickStartY, hourRight, tickStartY);
								if (endHere) {
									drawEndBorder();
								}
							}
						} else if (endHere) {
							if (tickEndY - scale > hourY) {
								fillRectBetween(selectBrush, hourX, hourY, hourRight, tickEndY - scale);
							}
							drawEndBorder();
						}

						//
						if (startHere) {
							//g.DrawRectangle(borderPen, tickStartX, tickStartY, scale, scale);
						}
						if (endHere) {
							//g.DrawRectangle(borderPen, tickEndX - scale, tickEndY - scale, scale, scale);
						}
					}

					string label;
					if (currHour == 0) {
						label = CalenCore.startTime.AddDays(fullDays).Day + "/";
					} else {
						label = currHour.ToString().PadLeft(2, '0') + ":";
					}
					if (++currHour >= 24) {
						currHour -= 24;
						fullDays += 1;
					}
					g.DrawString(label, font, fontBrush, hourX, hourY + hourHeight + 4);

					if (++col >= hoursPerRow) {
						col = 0;
						row += 1;
					}
					hourStart += CalenHour.ticksPerHour;
					hourEnd += CalenHour.ticksPerHour;
				}
			}
			canvasEl.Image = bitmap;

			if (updateTimeView) CalenTimeView.update();
		}

		public static void init() {
			bitmap = new Bitmap(canvasEl.Width, 100);
			canvasEl.Image = bitmap;
			redraw();
		}
	}
}
