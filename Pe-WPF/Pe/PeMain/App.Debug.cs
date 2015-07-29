namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	partial class App
	{
#if DEBUG
		/// <summary>
		/// デバッグ時にちょっとしたコード検証
		/// </summary>
		void DebugProcess()
		{
			//icon();
			//font();
		}

		void icon()
		{
			//var path = Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES%\Waterfox\waterfox.exe");
			var pathList = new[] {
				@"C:\Program Files\Waterfox\waterfox.exe",
				@"C:\Windows",
				@"C:\",
				@"C:\Program Files (x86)\IrfanView\i_view32.exe"
			};
			var saveDir = @"Z:\";

			foreach (var path in pathList.Select(Environment.ExpandEnvironmentVariables)) {
				foreach (var scale in new[] { IconScale.Small, IconScale.Normal, IconScale.Big, IconScale.Large }) {
					var image = IconUtility.Load(path, scale, 0);
					var name = string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(path), scale);
					var savePath = Path.Combine(saveDir, name);
					using (var fileStream = new FileStream(savePath, FileMode.Create)) {
						var encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
						encoder.Save(fileStream);
					}
				}
			}
		}

		void font()
		{
			var names = new[] { null, "", "Arial", "ＭＳ ゴシック" };
			foreach(var name in names) {
				var ff = FontUtility.MakeFontFamily(name, SystemFonts.MessageFontFamily);
				Debug.WriteLine(ff);
			}
		}


#endif
	}
}
