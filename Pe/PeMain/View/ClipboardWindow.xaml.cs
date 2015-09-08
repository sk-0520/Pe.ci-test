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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// ClipboardWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class ClipboardWindow : ViewModelCommonDataWindow<ClipboardViewModel>
	{
		public ClipboardWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			UIUtility.SetStyleToolWindow(this, false, false);

			base.OnLoaded(sender, e);
		}

		protected override void CreateViewModel()
		{
			ViewModel = new ClipboardViewModel(
				CommonData.MainSetting.Clipboard,
				this,
				CommonData.ClipboardIndexSetting,
				CommonData.NonProcess,
				CommonData.AppSender
			);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		#endregion

	}
}
