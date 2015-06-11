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
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// LoggingWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LoggingWindow : ViewModelCommonDataWindow<LoggingViewModel>
	{
		public LoggingWindow()
		{
			InitializeComponent();
		}

		#region CommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new LoggingViewModel(CommonData.MainSetting.Logging);
			ViewModel.View = this;
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;
		}

		#endregion
	}
}
