using System;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class HomeForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			AcceptButton = this.commandClose;
			CancelButton = this.commandClose;
			this.commandClose.SetLanguage(CommonData.Language);
			
			var controls = new Control[] {
				this.commandLauncher,
				this.labelLauncher,
				this.commandNotify,
				this.labelNotify,
				this.commandStartup,
				this.labelStartup,
			};
			foreach(var control in controls) {
				control.SetLanguage(CommonData.Language);
			}
			
		}
	}
}
