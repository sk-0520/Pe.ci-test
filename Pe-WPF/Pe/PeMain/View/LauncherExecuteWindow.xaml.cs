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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// LauncherExecuteWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherExecuteWindow: ViewModelCommonDataWindow<LauncherExecuteItemViewModel>, IHavingWindowKind
	{
		public LauncherExecuteWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = (LauncherItemModel)ExtensionData;
			ViewModel = new LauncherExecuteItemViewModel(model, this, CommonData.LauncherIconCaching, CommonData.NonProcess);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		#endregion

		#region IHavingWindowKind

		public WindowKind WindowKind { get { return WindowKind.LauncherExecute; } }

		#endregion
	}
}
