﻿namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
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
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// CommandWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class CommandWindow : ViewModelCommonDataWindow<CommandViewModel>
	{
		public CommandWindow()
		{
			InitializeComponent();
		}

		#region property

		CaptionCursorHitTest WindowHitTest { get; set; }
		VisualStyle VisualStyle { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new CommandViewModel(CommonData.MainSetting.Command, this, CommonData.LauncherItemSetting, CommonData.NonProcess, CommonData.AppSender);
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			int exStyle = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE);
			exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

			// ノート側はこれつけなきゃいけないの腑に落ちん
			var style = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_STYLE);
			style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_STYLE, (IntPtr)style);

			base.OnLoaded(sender, e);

			WindowHitTest = new WidthResizeHitTest(this, ViewModel, CommonData.NonProcess);
			VisualStyle = new VisualStyle(this, ViewModel, CommonData.NonProcess);
		}

		protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			//if(Appbar != null) {
			//	Appbar.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (VisualStyle != null) {
			//	VisualStyle.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (WindowAreaCorrection != null) {
			//	WindowAreaCorrection.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (handled) {
			//	return IntPtr.Zero;
			//}


			var extends = new IHavingWndProc[] {
				VisualStyle,
				WindowHitTest,
			};
			foreach(var extend in extends.Where(e => e != null)) {
				var result = extend.WndProc(hWnd, msg, wParam, lParam, ref handled);
				if(handled) {
					return result;
				}
			}

			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		#endregion
	}
}
