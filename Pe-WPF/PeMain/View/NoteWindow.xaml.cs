namespace ContentTypeTextNet.Pe.PeMain.View
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
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// NoteWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class NoteWindow: ViewModelCommonDataWindow<NoteViewModel>, IHavingWindowKind
	{
		public NoteWindow()
		{
			InitializeComponent();
		}

		#region property

		CaptionDoubleClick CaptionDoubleClick { get; set; }
		WindowHitTest WindowHitTest { get; set; }
		WindowAreaCorrection WindowAreaCorrection { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = (NoteIndexItemModel)ExtensionData;
			ViewModel = new NoteViewModel(model, this, CommonData.NonProcess, CommonData.AppSender);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			int exStyle = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE);
			exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

			var style = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_STYLE);
			style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_STYLE, (IntPtr)style); 
			
			base.OnLoaded(sender, e);

			CaptionDoubleClick = new CaptionDoubleClick(this, ViewModel, CommonData.NonProcess);
			//WindowAreaCorrection = new WindowAreaCorrection(this, ViewModel, CommonData.NonProcess);
			WindowHitTest = new CaptionCursorHitTest(this, ViewModel, CommonData.NonProcess);
		}

		protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var extends = new IHavingWndProc[] {
				CaptionDoubleClick,
				WindowAreaCorrection,
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

		#region IHavingWindowKind

		public WindowKind WindowKind { get { return WindowKind.Note; } }

		#endregion

		#region function

		//void ResetPopupPosition()
		//{
		//	//popup.HorizontalOffset += 1;
		//	//popup.HorizontalOffset -= 1;
		//}

		#endregion

		///// <summary>
		///// <para>http://stackoverflow.com/questions/5736359/popup-control-moves-with-parent</para>
		///// </summary>
		///// <param name="sender"></param>
		///// <param name="e"></param>
		//protected override void OnLocationChanged(EventArgs e)
		//{
		//	ResetPopupPosition();
		//	base.OnLocationChanged(e);
		//}

		//protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		//{
		//	ResetPopupPosition();
		//	base.OnRenderSizeChanged(sizeInfo);
		//}
	}
}
