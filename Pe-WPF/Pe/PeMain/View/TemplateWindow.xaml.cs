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
	/// TemplateWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class TemplateWindow : ViewModelCommonDataWindow<TemplateViewModel>
	{
		public TemplateWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new TemplateViewModel(
				CommonData.MainSetting.Template,
				this,
				CommonData.TemplateIndexSetting,
				CommonData.NonProcess,
				CommonData.ClipboardWatcher,
				CommonData.VariableConstants,
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
