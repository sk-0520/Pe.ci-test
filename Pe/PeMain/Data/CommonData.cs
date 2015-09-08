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

		public sealed class AppNonProcessImplement: IAppNonProcess
		{
			public AppNonProcessImplement()
			{ }

			public ILogger Logger { get; set; }
			public ILanguage Language { get; set; }
			public VariableConstants VariableConstants { get; set; }
			public LauncherIconCaching LauncherIconCaching { get; set; }
			public IClipboardWatcher ClipboardWatcher { get; set; }
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

		#region IAppNonProcess

		public AppLanguageManager Language { get; set; }
		public ILogger Logger { get; set; }
		public IAppSender AppSender { get; set; }
		public IClipboardWatcher ClipboardWatcher { get; set; }
		public LauncherIconCaching LauncherIconCaching { get; set; }

		#endregion

		/// <summary>
		/// 呼び出し元から見てると心臓に悪い。
		/// </summary>
		public IAppNonProcess NonProcess
		{ 
			get 
			{
				if(NonProcessInstance == null) {
					NonProcessInstance = new AppNonProcessImplement();
				}
				NonProcessInstance.Language = Language;
				NonProcessInstance.Logger = Logger;
				NonProcessInstance.LauncherIconCaching = LauncherIconCaching;
				NonProcessInstance.VariableConstants = VariableConstants;
				NonProcessInstance.ClipboardWatcher = ClipboardWatcher;

				return NonProcessInstance; 
			} 
		}

		#endregion

		#region DisposeFinalizeModelBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				if (MainSetting != null) {
					MainSetting.Dispose();
					MainSetting = null;
				}
				if (LauncherItemSetting != null) {
					LauncherItemSetting.Dispose();
					LauncherItemSetting = null;
				}
				if (LauncherGroupSetting != null) {
					LauncherGroupSetting.Dispose();
					LauncherGroupSetting = null;
				}
				if (Logger != null) {
					Logger.Dispose();
					Logger = null;
				}
			}
			base.Dispose(disposing);
		}

		#endregion

	}
}
