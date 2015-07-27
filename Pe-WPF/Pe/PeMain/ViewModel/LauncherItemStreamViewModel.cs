namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherItemStreamViewModel : LauncherItemSimpleViewModel, IHavingView<LauncherItemStreamWindow>
	{
		public LauncherItemStreamViewModel(LauncherItemModel model, LauncherItemStreamWindow view, Process process, StreamSettingModel streamSetting, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess, IAppSender appSender)
			: base(model, launcherIconCaching, nonPorocess, appSender)
		{
			View = view;
			Process = process;
			StreamSetting = streamSetting;
		}

		#region property

		Process Process{get;set;}
		StreamSettingModel StreamSetting {get;set;}

		#endregion

		#region function

		public void Start()
		{
		}

		#endregion

		#region IHavingView

		public LauncherItemStreamWindow View { get; private set; }

		public bool HasView { get { return View != null;} }

		#endregion
	}

}
