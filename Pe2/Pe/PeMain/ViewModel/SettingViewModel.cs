namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class SettingViewModel: ViewModelBase, IHavingCommonData, IHavingView<SettingWindow>
	{
		public SettingViewModel(SettingWindow view, CommonData commonData)
		{
			CommonData = commonData;
			View = view;
		}

		#region property

		#region IHavingCommonData

		public CommonData CommonData { get; private set; }

		#endregion

		#region IHavingView

		public SettingWindow View { get; private set; }
		public bool HasView { get { return HavingViewUtility.GetHasView(this); } }

		#endregion

		#endregion

		#region command

		public ICommand CancelCommand
		{
			get
			{
				var reslut = CreateCommand(
					o => {
						View.Close();
					}
				);

				return reslut;
			}
		}

		#endregion
	}
}
