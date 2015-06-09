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
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// AcceptWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class AcceptWindow: CommonDataWindow
	{
		public AcceptWindow()
		{
			InitializeComponent();
		}

		#region CommonDataWindow

		protected override void ApplyViewModel()
		{
			var vm = new AcceptViewModel(CommonData.MainSetting.RunningInformation);
			DataContext = vm;

			base.ApplyViewModel();
		}

		#endregion

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{

		}
	}
}
