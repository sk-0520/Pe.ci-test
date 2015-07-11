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
			public AppNonProcessImplement()
			{ }

			public ILogger Logger { get; set; }
			public ILanguage Language { get; set; }
		}

		#endregion

		public CommonData()
			: base()
		{
			LauncherIconCaching = new LauncherIconCaching();
		}

		#region property

		AppNonProcessImplement NonProcessInstance { get; set; }

		public VariableConstants VariableConstants { get; set; }

		public MainSettingModel MainSetting { get; set; }
		public LauncherItemSettingModel LauncherItemSetting { get; set; }
		public LauncherGroupSettingModel LauncherGroupSetting { get; set; }
		public NoteIndexSettingModel NoteIndexSetting { get; set; }
		public ClipboardIndexSettingModel ClipboardIndexSetting { get; set; }
		public TemplateIndexSettingModel TemplateIndexSetting { get; set; }
		public AppLanguageManager Language { get; set; }

		public ILogger Logger { get; set; }
		public IAppSender AppSender { get; set; }
		public IClipboardWatcher ClipboardWatcher { get; set; }

		public LauncherIconCaching LauncherIconCaching { get; set; }

		/// <summary>
		/// 呼び出し元から見てると心臓に悪い。
		/// </summary>
		public INonProcess NonProcess
		{ 
			get 
			{
				if(NonProcessInstance == null) {
					NonProcessInstance = new AppNonProcessImplement();
				}
				NonProcessInstance.Language = Language;
				NonProcessInstance.Logger = Logger;

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
