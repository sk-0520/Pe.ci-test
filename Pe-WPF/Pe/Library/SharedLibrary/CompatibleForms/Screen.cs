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
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;

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
				DeviceBounds = DrawingUtility.Convert(screen.Bounds),
				DeviceName = screen.DeviceName,
				Primary = screen.Primary,
				DeviceWorkingArea = DrawingUtility.Convert(screen.WorkingArea),
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

		/// <summary>
		/// 指定したポイントを保持するディスプレイを表す ScreenModel を取得します。
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static ScreenModel FromDevicePoint(Point point)
		{
			var drawingPoint = DrawingUtility.Convert(point);
			var formScreen = Forms.Screen.FromPoint(drawingPoint);
			var result = ConvertScreenModel(formScreen);

			return result;
		}

		/// <summary>
		/// 四角形の最大部分を保持する ScreenModel を取得します。
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static ScreenModel FromDeviceRectangle(Rect rect)
		{
			var drawingRect = DrawingUtility.Convert(rect);
			var formScreen = Forms.Screen.FromRectangle(drawingRect);
			var result = ConvertScreenModel(formScreen);

			return result;
		}

		/// <summary>
		/// 指定したハンドルによって参照されているオブジェクトの最大領域を保持するディスプレイを表す ScreenModel を取得します。
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public static ScreenModel FromHandle(IntPtr hWnd)
		{
			var formScreen = Forms.Screen.FromHandle(hWnd);

			var result = ConvertScreenModel(formScreen);

			return result;
		}

	}
}
