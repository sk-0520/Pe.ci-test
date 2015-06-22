namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public sealed class CommonData: DisposeFinalizeModelBase, INonProcess
	{
		public CommonData()
			: base()
		{
			LauncherIcons = new LauncherIconCaching();
		}

		#region property

		public VariableConstants VariableConstants { get; set; }

		public MainSettingModel MainSetting { get; set; }
		public LauncherItemSettingModel LauncherItemSetting { get; set; }
		public LauncherGroupSettingModel LauncherGroupSetting { get; set; }
		public AppLanguageManager Language { get; set; }
		ILanguage INonProcess.Language { get { return Language; } }

		public ILogger Logger { get; set; }
		public IAppSender AppSender { get; set; }

		public LauncherIconCaching LauncherIcons { get; set; }

		/// <summary>
		/// 呼び出し元から見てると心臓に悪い。
		/// </summary>
		public INonProcess NonProcess { get { return this; } }

		#endregion

		#region DisposeFinalizeModelBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				MainSetting.Dispose();
				LauncherItemSetting.Dispose();
				LauncherGroupSetting.Dispose();
				Logger.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

	}
}
