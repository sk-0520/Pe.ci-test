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
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		#endregion

		#region function


		#endregion
	}
}
