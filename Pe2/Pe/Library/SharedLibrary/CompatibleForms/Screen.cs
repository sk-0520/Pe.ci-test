namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using Forms = System.Windows.Forms;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Extension;

	/// <summary>
	/// System.Windows.Forms.Screen 互換クラス。
	/// </summary>
	public static class Screen
	{
		/// <summary>
		/// System.Windows.Forms.Screen を ScreenModel に変換。
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		static ScreenModel ConvertScreenModel(Forms.Screen screen)
		{
			return new ScreenModel() {
				BitsPerPixel = screen.BitsPerPixel,
				DeviceBounds = screen.Bounds.ToRect(),
				DeviceName = screen.DeviceName,
				Primary = screen.Primary,
				DeviceWorkingArea = screen.WorkingArea.ToRect(),
			}; 
		}

		static IEnumerable< ScreenModel> GetAllScreens()
		{
			return Forms.Screen.AllScreens
				.Select(s => ConvertScreenModel(s))
			;
		}

		/// <summary>
		/// システム上のすべてのディスプレイの配列を取得します。
		/// </summary>
		public static ScreenModel[] AllScreens
		{
			get { return GetAllScreens().ToArray(); }
		}

		/// <summary>
		/// プライマリ ディスプレイを取得します。
		/// </summary>
		public static ScreenModel PrimaryScreen
		{
			get { return ConvertScreenModel(Forms.Screen.PrimaryScreen); }
		}

	}
}
