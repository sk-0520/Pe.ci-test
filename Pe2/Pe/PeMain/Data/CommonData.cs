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

		public sealed class NonProcessImplement: INonProcess
		{
			public bool initialized = false;

			public ILanguage Language { get; set; }
			public ILogger Logger { get; set; }
		}

		#endregion

		public CommonData()
			: base()
		{
			LauncherIcons = new LauncherIconCaching();
			
			NonProcessInstance = new NonProcessImplement();
		}

		#region property

		NonProcessImplement NonProcessInstance { get; set; }

		public VariableConstants VariableConstants { get; set; }

		public MainSettingModel MainSetting { get; set; }
		public LauncherItemSettingModel LauncherItemSetting { get; set; }
		public LauncherGroupSettingModel LauncherGroupSetting { get; set; }
		public AppLanguageManager Language { get; set; }

		public ILogger Logger { get; set; }
		public IAppSender AppSender { get; set; }

		public LauncherIconCaching LauncherIcons { get; set; }

		/// <summary>
		/// 呼び出し元から見てると心臓に悪い。
		/// </summary>
		public INonProcess NonProcess
		{ 
			get 
			{
				if(!NonProcessInstance.initialized) {
					NonProcessInstance.Language = Language;
					NonProcessInstance.Logger = Logger;
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
