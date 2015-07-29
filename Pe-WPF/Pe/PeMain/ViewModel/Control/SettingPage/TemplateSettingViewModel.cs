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
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class TemplateSettingViewModel : SettingPageViewModelBase
	{
		public TemplateSettingViewModel(TemplateSettingModel template, IAppNonProcess nonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, settingNotifiyItem)
		{
			Template = template;
		}

		#region property

		TemplateSettingModel Template { get; set; }

		#endregion
	}
}
