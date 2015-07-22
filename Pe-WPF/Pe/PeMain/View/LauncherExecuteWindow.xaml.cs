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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;

namespace ContentTypeTextNet.Pe.PeMain.View
{
	/// <summary>
	/// LauncherExecuteWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherExecuteWindow : ViewModelCommonDataWindow<LauncherExecuteItemViewModel>
	{
		public LauncherExecuteWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = (LauncherItemModel)ExtensionData;
			ViewModel = new LauncherExecuteItemViewModel(model, CommonData.LauncherIconCaching, CommonData.NonProcess);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		#endregion
	}
}
