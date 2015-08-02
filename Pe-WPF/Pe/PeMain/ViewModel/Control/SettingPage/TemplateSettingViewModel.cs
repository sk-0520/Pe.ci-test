namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;

	public class TemplateSettingViewModel : SettingPageViewModelBase
	{
		public TemplateSettingViewModel(TemplateSettingModel template, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			Template = template;
		}

		#region property

		TemplateSettingModel Template { get; set; }

		public bool IsTopmost
		{
			get { return Template.IsTopmost; }
			set { SetPropertyValue(Template, value); }
		}

		public bool IsVisible
		{
			get { return Template.IsVisible; }
			set { SetPropertyValue(Template, value); }
		}

		public HotKeyModel ToggleHotKey
		{
			get { return Template.ToggleHotKey; }
			set { SetPropertyValue(Template, value); }
		}

		#endregion
	}
}
