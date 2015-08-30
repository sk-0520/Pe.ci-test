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
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// StreamWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherItemStreamWindow : ViewModelCommonDataWindow<LauncherItemStreamViewModel>, IHavingWindowKind
	{
		public LauncherItemStreamWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var streamData = (StreamData)ExtensionData;
			ViewModel = new LauncherItemStreamViewModel(streamData.Model, this, streamData.Screen, streamData.Process, CommonData.MainSetting.Stream, CommonData.NonProcess, CommonData.AppSender);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		public override void ApplyLanguage(Dictionary<string, string> map)
		{
			map[LanguageKey.streamItem] = ViewModel.DisplayText;

			base.ApplyLanguage(map);
		}


		#endregion

		#region IHavingWindowKind

		public WindowKind WindowKind { get { return WindowKind.LauncherStream; } }

		#endregion
	}
}
