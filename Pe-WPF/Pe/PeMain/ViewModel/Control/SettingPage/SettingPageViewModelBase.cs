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

	public abstract class SettingPageViewModelBase: ViewModelBase, IHavingNonProcess
	{
		public SettingPageViewModelBase(INonProcess nonProcess)
		{
			CheckUtility.EnforceNotNull(nonProcess);

			NonProcess = nonProcess;
		}

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion
	}
}
