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
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// NoteWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class NoteWindow: ViewModelCommonDataWindow<NoteViewModel>
	{
		public NoteWindow()
		{
			InitializeComponent();
		}

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = (NoteItemModel)ExtensionData;
			ViewModel = new NoteViewModel(model, this, CommonData.NonProcess, CommonData.ClipboardWatcher);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			base.ApplyViewModel();
		}

		#endregion
	}
}
