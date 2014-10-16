using System;
using System.Windows.Forms;
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
		
		void CommandNotify_Click(object sender, EventArgs e)
		{
			SystemExecuter.OpenNotificationAreaHistory(CommonData);
		}
	}
}
