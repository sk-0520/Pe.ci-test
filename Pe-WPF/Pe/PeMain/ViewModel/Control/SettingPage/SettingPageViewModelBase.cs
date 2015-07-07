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
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class SettingPageViewModelBase: ViewModelBase, IHavingNonProcess, IHavingVariableConstants
	{
		public SettingPageViewModelBase(INonProcess nonProcess, VariableConstants variableConstants)
		{
			CheckUtility.EnforceNotNull(nonProcess);
			CheckUtility.EnforceNotNull(variableConstants);

			NonProcess = nonProcess;
			VariableConstants = variableConstants;
		}

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingVariableConstants

		public VariableConstants VariableConstants { get; private set; }

		#endregion
	}
}
