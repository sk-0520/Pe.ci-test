namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
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
	using System.Windows.Shapes;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using System.Windows.Threading;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>
	{
		public LauncherToolbarWindow()
		{
			InitializeComponent();
		}

		public LauncherToolbarWindow(ScreenModel screen)
			: this()
		{
			if (screen == null) {
				throw new ArgumentNullException("screen");
			}
			Screen = screen;
		}

		#region property

		ScreenModel Screen { get; set; }
		IApplicationDesktopToolbarData Appbar { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = new LauncherToolbarItemModel() {
				LauncherItems = CommonData.LauncherItemSetting.Items,
				GroupItems = CommonData.LauncherGroupItemSetting.Items,
			};

			ToolbarItemModel toolbar;
			if (Screen != null) {
				if(!CommonData.MainSetting.Toolbar.TryGetValue(Screen.DeviceName, out toolbar)) {
					toolbar = new ToolbarItemModel();
					toolbar.Id = Screen.DeviceName;
					CommonData.Logger.Information("create toolbar", Screen);
					CommonData.MainSetting.Toolbar.Add(toolbar);
				}
			} else {
				CommonData.Logger.Debug("dummy toolbar");
				toolbar = new ToolbarItemModel();
			}
			model.Toolbar = toolbar;
			
			ViewModel = new LauncherToolbarViewModel(model, this);

			// 以降Viewの保持するスクリーン情報は使用しない
			Screen = null;
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		#endregion

		#region function

		bool RegistAppbar()
		{
			Appbar.CallbackMessage = NativeMethods.RegisterWindowMessage(Appbar.MessageString);

			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = Appbar.CallbackMessage;

			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);

			return registResult.ToInt32() != 0;
		}

		bool UnResistAppbar()
		{
			var appBar = new APPBARDATA(Handle);
			var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			Appbar.CallbackMessage = 0;

			return unregistResult.ToInt32() != 0;
		}

		//private RECT CalcWantBarArea(DockType dockType)
		//{
		//	Debug.Assert(dockType != DockType.None);

		//	var desktopArea = Appbar.DockScreen.DeviceBounds;
		//	var barArea = new RECT();

		//	// 設定値からバー領域取得
		//	if (dockType == DockType.Left || dockType == DockType.Right) {
		//		barArea.Top = desktopArea.Top;
		//		barArea.Bottom = desktopArea.Bottom;
		//		if (dockType == DockType.Left) {
		//			barArea.Left = desktopArea.Left;
		//			barArea.Right = desktopArea.Left + BarSize.Width;
		//		} else {
		//			barArea.Left = desktopArea.Right - BarSize.Width;
		//			barArea.Right = desktopArea.Right;
		//		}
		//	} else {
		//		Debug.Assert(dockType == DockType.Top || dockType == DockType.Bottom);
		//		barArea.Left = desktopArea.Left;
		//		barArea.Right = desktopArea.Right;
		//		if (dockType == DockType.Top) {
		//			barArea.Top = desktopArea.Top;
		//			barArea.Bottom = desktopArea.Top + BarSize.Height;
		//		} else {
		//			barArea.Top = desktopArea.Bottom - BarSize.Height;
		//			barArea.Bottom = desktopArea.Bottom;
		//		}
		//	}

		//	return barArea;
		//}


		#endregion
	}
}
