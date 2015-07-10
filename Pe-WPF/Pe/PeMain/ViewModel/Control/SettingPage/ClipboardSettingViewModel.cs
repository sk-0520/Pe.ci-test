namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class ClipboardSettingViewModel : SettingPageViewModelBase
	{
		public ClipboardSettingViewModel(ClipboardSettingModel clipboard, INonProcess nonProcess, VariableConstants variableConstants, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, variableConstants, settingNotifiyItem)
		{
			Clipboard = clipboard;
		}

		#region property

		ClipboardSettingModel Clipboard { get; set; }

		#endregion
	}
}
