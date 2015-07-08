namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class SettingPageViewModelBase: ViewModelBase, IHavingNonProcess, IHavingVariableConstants
	{
		public SettingPageViewModelBase(INonProcess nonProcess, VariableConstants variableConstants, SettingNotifiyItem settingNotifiyItem)
		{
			CheckUtility.DebugEnforceNotNull(nonProcess);
			CheckUtility.DebugEnforceNotNull(variableConstants);
			CheckUtility.DebugEnforceNotNull(settingNotifiyItem);

			NonProcess = nonProcess;
			VariableConstants = variableConstants;
			SettingNotifiyItem = settingNotifiyItem;
		}

		#region property

		protected SettingNotifiyItem SettingNotifiyItem { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingVariableConstants

		public VariableConstants VariableConstants { get; private set; }

		#endregion
	}
}
