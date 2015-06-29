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
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// SettingWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class SettingWindow : CommonDataWindow
	{
		public SettingWindow()
		{
			InitializeComponent();
		}

		#region CommonDataWindow

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();
			//TODO
			var clonedCommonData = CommonData;
			DataContext = new SettingViewModel(clonedCommonData, this);
		}

		#endregion
	}
}
