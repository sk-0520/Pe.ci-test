namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class SettingPageViewModelBase<TView>: ViewModelBase, IHavingAppNonProcess, IHavingView<TView>
		where TView: UserControl
	{
		public SettingPageViewModelBase(TView view, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
		{
			CheckUtility.DebugEnforceNotNull(view);
			CheckUtility.DebugEnforceNotNull(appNonProcess);
			CheckUtility.DebugEnforceNotNull(settingNotifiyItem);

			View = view;
			AppNonProcess = appNonProcess;
			SettingNotifiyItem = settingNotifiyItem;
		}

		#region property

		protected SettingNotifiyItem SettingNotifiyItem { get; private set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region IHavingView

		public TView View { get; private set; }

		public bool HasView
		{
			get
			{
				return HavingViewUtility.GetHasView(this);
			}
		}

		#endregion
	}
}
