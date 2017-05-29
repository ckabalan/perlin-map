using System;
using System.Drawing;
using System.IO;

namespace PerlinMap {
	public static class BitmapExtensionMethods {
		public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point, Bitmap> action) {
			Point point = new Point(0, 0);
			for (int x = 0; x < bitmap.Width; x++) {
				point.X = x;
				for (int y = 0; y < bitmap.Height; y++) {
					point.Y = y;
					action(point, bitmap);
				}
			}
		}

		public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point> action) {
			Point point = new Point(0, 0);
			for (int x = 0; x < bitmap.Width; x++) {
				point.X = x;
				for (int y = 0; y < bitmap.Height; y++) {
					point.Y = y;
					action(point);
				}
			}
		}

		public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color> colourFunc) {
			Point point = new Point(0, 0);
			for (int x = 0; x < bitmap.Width; x++) {
				point.X = x;
				for (int y = 0; y < bitmap.Height; y++) {
					point.Y = y;
					bitmap.SetPixel(x, y, colourFunc(point));
				}
			}
		}

		public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color, Color> colourFunc) {
			Point point = new Point(0, 0);
			for (int x = 0; x < bitmap.Width; x++) {
				point.X = x;
				for (int y = 0; y < bitmap.Height; y++) {
					point.Y = y;
					bitmap.SetPixel(x, y, colourFunc(point, bitmap.GetPixel(x, y)));
				}
			}
		}

		public static System.Windows.Media.Imaging.BitmapImage Bitmap2BitmapImage(this Bitmap bitmap) {
			using (MemoryStream ms = new MemoryStream()) {
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;
				System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
				bi.BeginInit();
				bi.StreamSource = ms;
				bi.EndInit();
				return bi;
			}
		}

	}
}
