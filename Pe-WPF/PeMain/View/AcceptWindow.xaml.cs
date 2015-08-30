namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.IO;
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
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// AcceptWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class AcceptWindow: ViewModelCommonDataWindow<AcceptViewModel>
	{
		public AcceptWindow()
		{
			InitializeComponent();
		}

		#region CommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new AcceptViewModel(CommonData.MainSetting.RunningInformation, this);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			ViewModel.SetAcceptDocument(webDocument, CommonData.Language, CommonData.VariableConstants);

			base.ApplyViewModel();
		}

		#endregion
	}
}
