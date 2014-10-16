using System;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class HomeForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			AcceptButton = this.commandClose;
			CancelButton = this.commandClose;
			commandClose.SetLanguage(CommonData.Language);
			
			this.tabHome_pageMain.SetLanguage(CommonData.Language);
		}
	}
}
