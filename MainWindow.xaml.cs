using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PerlinMap {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			MT19937.Seed(Convert.ToUInt32((new Random(1)).Next() * 100));
			Console.WriteLine(MT19937.Random());
		}

		private void GenerateBtn_Click(object sender, RoutedEventArgs e) {
			PerlinNoise perlinNoise = new PerlinNoise(MT19937.Random((new Random(1)).Next()));
			Bitmap bitmap = new Bitmap(Convert.ToInt32(PerlinMapImage.Width), Convert.ToInt32(PerlinMapImage.Height));
			double widthDivisor = 1 / (double)PerlinMapImage.Width;
			double heightDivisor = 1 / (double)PerlinMapImage.Height;
			bitmap.SetEachPixelColour(
				(point, color) => {
					// Note that the result from the noise function is in the range -1 to 1, but I want it in the range of 0 to 1
					// that's the reason of the strange code
					double v =
						// First octave
						(perlinNoise.Noise(2 * point.X * widthDivisor, 2 * point.Y * heightDivisor, -0.5) + 1) / 2 * 0.7 +
						// Second octave
						(perlinNoise.Noise(4 * point.X * widthDivisor, 4 * point.Y * heightDivisor, 0) + 1) / 2 * 0.5 +
						// Third octave
						(perlinNoise.Noise(8 * point.X * widthDivisor, 8 * point.Y * heightDivisor, +0.5) + 1) / 2 * 0.3 +
						// Fourth octave
						(perlinNoise.Noise(16 * point.X * widthDivisor, 16 * point.Y * heightDivisor, +0.5) + 1) / 2 * 0.1;

					v = Math.Min(1, Math.Max(0, v));
					byte b = (byte)(v * 255);
					return System.Drawing.Color.FromArgb(b, b, b);
				});

			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			ms.Seek(0, SeekOrigin.Begin);
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = ms;
			bi.EndInit();
			PerlinMapImage.Source = bi;
		}

	}
}
