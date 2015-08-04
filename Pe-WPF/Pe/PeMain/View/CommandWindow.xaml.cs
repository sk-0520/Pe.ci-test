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

namespace ContentTypeTextNet.Pe.PeMain.View
{
	/// <summary>
	/// CommandWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class CommandWindow : ViewModelCommonDataWindow<CommandViewModel>
	{
		public CommandWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new CommandViewModel(CommonData.MainSetting.Command, this, CommonData.LauncherItemSetting, CommonData.NonProcess);
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		#endregion
	}
}
