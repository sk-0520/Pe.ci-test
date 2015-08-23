namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class AboutViewModel: HavingViewModelBase<AboutWindow>
	{
		public AboutViewModel(AboutWindow view, AboutNotifiyItem notifiy)
			: base(view)
		{
			CheckUtility.DebugEnforceNotNull(notifiy);

			Notifiy = notifiy;
		}

		#region property

		AboutNotifiyItem Notifiy { get; set; }

		#endregion
	}
}
