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

	public sealed class CommonData: DisposeFinalizeModelBase
	{
		#region define

		public sealed class AppNonProcessImplement: INonProcess
		{
			public AppNonProcessImplement(ILogger logger, ILanguage language)
			{
				Logger = logger;
				Language = language;
			}

			public ILogger Logger { get; private set; }
			public ILanguage Language { get; private set; }
		}

		#endregion

		public CommonData()
			: base()
		{
			LauncherIcons = new LauncherIconCaching();
		}

		#region property

		AppNonProcessImplement NonProcessInstance { get; set; }

		public VariableConstants VariableConstants { get; set; }

		public MainSettingModel MainSetting { get; set; }
		public LauncherItemSettingModel LauncherItemSetting { get; set; }
		public LauncherGroupSettingModel LauncherGroupSetting { get; set; }
		public AppLanguageManager Language { get; set; }

		public ILogger Logger { get; set; }
		public IAppSender AppSender { get; set; }
		public IClipboardWatcher ClipboardWatcher { get; set; }

		public LauncherIconCaching LauncherIcons { get; set; }

		/// <summary>
		/// 呼び出し元から見てると心臓に悪い。
		/// </summary>
		public INonProcess NonProcess
		{ 
			get 
			{
				if(NonProcessInstance == null) {
					NonProcessInstance = new AppNonProcessImplement(Logger, Language);
				}

				return NonProcessInstance; 
			} 
		}

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
